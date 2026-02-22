using ECommerce.Application.DTOs.Order;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> PlaceOrder(Guid userId, CheckoutRequestDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var cartItems = await _cartRepository.GetUserCart(userId);
                if (!cartItems.Any())
                    throw new Exception("Cart is Empty");

                decimal total = 0;
                var orderItems = new List<OrderItem>();

                foreach (var cart in cartItems)
                {
                    var product = await _productRepository.GetByIdAsync(cart.ProductId);

                    if (product == null || product.Stock < cart.Quantity)
                        throw new Exception($"Insufficient stock for {cart.Product.Name}");

                    if (cart.Quantity <= 0)
                        throw new Exception("Invalid quantity");

                    if (cart.Quantity > product.MaxOrderQuantity)
                        throw new Exception(
                            $"You can order only {product.MaxOrderQuantity} units of {product.Name}");

                    if (cart.Quantity > product.Stock)
                        throw new Exception(
                            $"Only {product.Stock} units available for {product.Name}");

                    // Reserve stock immediately
                    product.Stock -= cart.Quantity;
                    await _productRepository.UpdateAsync(product);

                    total += cart.Product.Price * cart.Quantity;

                    orderItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = cart.Quantity
                    });
                }

                // Order starts as Pending — it becomes Placed when payment is confirmed
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    ShippingAddress = dto.ShippingAddress,
                    TotalAmount = total,
                    Status = OrderStatus.Pending,
                    OrderItems = orderItems
                };

                await _orderRepository.AddAsync(order);

                await _unitOfWork.CommitAsync();

                // Return orderId — frontend uses it when calling POST /payments
                return order.Id;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<List<OrderResponseDto>> GetUserOrders(Guid userId)
        {
            var orders = await _orderRepository.GetUserOrders(userId);

            return orders.Select(o => new OrderResponseDto
            {
                OrderId = o.Id,
                TotalAmount = o.TotalAmount,
                Status = o.Status.ToString(),
                ShippingAdress = o.ShippingAddress,
                CreatedAt = o.CreatedAt,
                Items = o.OrderItems.Select(i => new UserOrderItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            }).ToList();
        }

        public async Task CancelOrder(Guid userId,Guid orderId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {



                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null || order.UserId != userId)
                    throw new Exception("Order not found");

                if (order.Status != OrderStatus.Pending &&
                    order.Status != OrderStatus.Placed)
                {
                    throw new Exception("Order cannot be cancelled");
                }

                //if we cancel the order we want to restore the stoke

                foreach (var item in order.OrderItems)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.Stock += item.Quantity;
                        await _productRepository.UpdateAsync(product);
                    }
                }

                order.Status = OrderStatus.Cancelled;
                await _orderRepository.UpdateAsync(order);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

    }
}
