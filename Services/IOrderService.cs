using APIPedidos.Models;
using APIPedidos.DTOs;

namespace APIPedidos.Services;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int id);
    Task<Order> CreateOrderAsync();
    Task<OrderItem> AddItemToOrderAsync(int orderId, int productId, int quantity);
    Task<bool> UpdateOrderStateAsync(int orderId, OrderState newState);
    Task<bool> IsProductInAnyOrderAsync(int productId);
} 