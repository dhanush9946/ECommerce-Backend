

using ECommerce.Application.DTOs.Order;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Application.Services
{
    public class AdminOrderService:IAdminOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminOrderService(IOrderRepository orderRepository,
                                 IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        
        public async Task<List<AdminOrderResponseDto>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllAsync();

            return orders.Select(o => new AdminOrderResponseDto
            {
                OrderId = o.Id,
                UserId = o.UserId,
                TotalAmount = o.TotalAmount,
                Status = o.Status.ToString(),
                ShippingAddress = o.ShippingAddress,
                CreatedAt = o.CreatedAt,
                Items = o.OrderItems.Select(i => new AdminOrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            }).ToList();
        }

        public async Task<AdminOrderResponseDto> GetOrderById(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new Exception("Order not found");

            return new AdminOrderResponseDto
            {
                OrderId = order.Id,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                ShippingAddress = order.ShippingAddress,
                CreatedAt = order.CreatedAt,
                Items = order.OrderItems.Select(i => new AdminOrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            };
        }
        public async Task UpdateOrderStatus(Guid orderId, string status)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                    throw new Exception("Order not found");

                
                if (order.Status == OrderStatus.Delivered)
                    throw new Exception("Delivered order cannot be updated");

                if (order.Status == OrderStatus.Cancelled)
                    throw new Exception("Cancelled order cannot be updated");

                //  Convert string to enum
                if (!Enum.TryParse<OrderStatus>(
                        status,
                        ignoreCase: true,
                        out var newStatus))
                {
                    throw new Exception("Invalid order status");
                }

                //  enforce valid transitions
                ValidateStatusTransition(order.Status, newStatus);

                
                order.Status = newStatus;

                await _orderRepository.UpdateAsync(order);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }


        private void ValidateStatusTransition(OrderStatus current, OrderStatus next)
        {
            if (current == OrderStatus.Placed && next == OrderStatus.Delivered)
                throw new Exception("Order must be confirmed before delivery");

            if (current == OrderStatus.Confirmed && next == OrderStatus.Placed)
                throw new Exception("Cannot revert order to Placed");

            if (current == OrderStatus.Shipped && next == OrderStatus.Confirmed)
                throw new Exception("Cannot revert shipped order");
        }

    }
}
