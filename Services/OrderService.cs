using APIPedidos.Models;
using APIPedidos.DTOs;

namespace APIPedidos.Services;

public class OrderService : IOrderService
{
    private readonly IProductService _productService;
    private readonly IProductValidationService _validationService;
    private readonly List<Order> _orders = new();
    private int _nextOrderId = 1;
    private int _nextOrderItemId = 1;

    public OrderService(IProductService productService, IProductValidationService validationService)
    {
        _productService = productService;
        _validationService = validationService;
    }

    public Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return Task.FromResult(_orders.AsEnumerable());
    }

    public Task<Order?> GetOrderByIdAsync(int id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        return Task.FromResult(order);
    }

    public Task<Order> CreateOrderAsync()
    {
        var order = new Order
        {
            Id = _nextOrderId++,
            CreatedAt = DateTime.UtcNow,
            State = OrderState.Pendiente,
            Items = new List<OrderItem>()
        };

        _orders.Add(order);
        UpdateValidationService();
        return Task.FromResult(order);
    }

    public async Task<OrderItem> AddItemToOrderAsync(int orderId, int productId, int quantity)
    {
        var order = await GetOrderByIdAsync(orderId);
        if (order == null)
            throw new ArgumentException("Order not found");

        var product = await _productService.GetProductByIdAsync(productId);
        if (product == null)
            throw new ArgumentException("Product not found");

        // Regla de Negocio Crítica 2: No permitir agregar un producto que ya existe en la orden
        if (order.Items.Any(item => item.ProductId == productId))
            throw new InvalidOperationException("Product already exists in this order");

        var orderItem = new OrderItem
        {
            Id = _nextOrderItemId++,
            OrderId = orderId,
            ProductId = productId,
            Product = product,
            Quantity = quantity,
            UnitPrice = product.Price
        };

        order.Items.Add(orderItem);
        UpdateValidationService();
        return orderItem;
    }

    public async Task<bool> UpdateOrderStateAsync(int orderId, OrderState newState)
    {
        var order = await GetOrderByIdAsync(orderId);
        if (order == null)
            return false;

        order.State = newState;
        return true;
    }

    public Task<bool> IsProductInAnyOrderAsync(int productId)
    {
        var isInAnyOrder = _orders.Any(order => order.Items.Any(item => item.ProductId == productId));
        // Retorna false si el producto está en alguna orden (no se puede eliminar)
        // Retorna true si el producto NO está en ninguna orden (se puede eliminar)
        return Task.FromResult(!isInAnyOrder);
    }

    private void UpdateValidationService()
    {
        if (_validationService is ProductValidationService validationService)
        {
            validationService.SetOrders(_orders);
        }
    }
} 