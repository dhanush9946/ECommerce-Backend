

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
                Status = o.Status,
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
                Status = order.Status,
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
        public async Task UpdateOrderStatus(Guid userId,string status)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var order = await _orderRepository.GetByIdAsync(userId);
                if (order == null)
                    throw new Exception("Order Not Found");

                if (order.Status == "Delivered")
                    throw new Exception("Delivered order cannot be updated");
                if (order.Status == "Cancelled")
                    throw new Exception("Cancelled order cannot be updated");

                var allowedStatuses = new[]
                {
                    "Placed", "Confirmed", "Shipped", "Delivered", "Cancelled"
                };

                if (!allowedStatuses.Contains(status))
                    throw new Exception("Invalid order status");



                order.Status = status;
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
