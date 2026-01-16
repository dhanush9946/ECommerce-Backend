using ECommerce.Application.DTOs.Order;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class OrderService:IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public OrderService(IOrderRepository orderRepository,
                            ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        public async Task<Guid> Checkout(Guid userId,CheckoutRequestDto dto)
        {
            var cartItems = await _cartRepository.GetUserCart(userId);
            if (!cartItems.Any())
                throw new Exception("Cart is Empty");

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TotalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity),
                OrderItems = cartItems.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    ProductName = c.Product.Name,
                    Price = c.Product.Price,
                    Quantity = c.Quantity
                }).ToList()
            };

            await _orderRepository.AddAsync(order);

            foreach(var item in cartItems)
            {
                await _cartRepository.RemoveAsync(item);
            }
            return order.Id;
        }

        public async Task<List<OrderResponseDto>> GetUserOrders(Guid userId)
        {
            var orders = await _orderRepository.GetUserOrders(userId);

            return orders.Select(o => new OrderResponseDto
            {
                OrderId = o.Id,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedAt = o.CreatedAt
            }).ToList();
        }

    }
}
