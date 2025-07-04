using APIPedidos.Data;
using APIPedidos.DTOs;
using APIPedidos.Models;
using Microsoft.EntityFrameworkCore;

namespace APIPedidos.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly IProductService _productService;
    private readonly IProductValidationService _validationService;

    public OrderService(
        ApplicationDbContext context,
        IProductService productService,
        IProductValidationService validationService
    )
    {
        _context = context;
        _productService = productService;
        _validationService = validationService;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context
            .Orders.Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _context
            .Orders.Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> CreateOrderAsync()
    {
        var order = new Order
        {
            CreatedAt = DateTime.UtcNow,
            State = OrderState.Pendiente,
            Items = new List<OrderItem>(),
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<OrderItem> AddItemToOrderAsync(int orderId, int productId, int quantity)
    {
        var order = await _context
            .Orders.Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            throw new ArgumentException(ValidationMessages.OrderNotFound);
        }

        if (order.State == OrderState.Enviado)
        {
            throw new InvalidOperationException(ValidationMessages.OrderAlreadyShipped);
        }

        // Regla de Negocio: No permitir agregar un producto que ya existe en la orden
        var existingItem = order.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            throw new InvalidOperationException(ValidationMessages.OrderItemProductExists);
        }

        var product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            throw new ArgumentException(ValidationMessages.OrderItemProductNotFound);
        }

        // Regla de Negocio: Verificar que hay suficiente stock
        if (product.StockQuantity < quantity)
        {
            throw new InvalidOperationException(ValidationMessages.OrderItemInsufficientStock);
        }

        var orderItem = new OrderItem
        {
            OrderId = orderId,
            ProductId = productId,
            Product = product,
            Quantity = quantity,
            UnitPrice = product.Price,
            Subtotal = product.Price * quantity,
        };

        // Actualizar stock del producto
        product.StockQuantity -= quantity;

        order.Items.Add(orderItem);
        order.Total = order.Items.Sum(i => i.Subtotal);

        await _context.SaveChangesAsync();
        return orderItem;
    }

    public async Task<bool> UpdateOrderStateAsync(int orderId, OrderState newState)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
            return false;

        if (order.State == OrderState.Enviado)
        {
            throw new InvalidOperationException(ValidationMessages.OrderAlreadyShipped);
        }

        order.State = newState;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
            return false;

        if (order.State == OrderState.Enviado)
        {
            throw new InvalidOperationException(ValidationMessages.OrderAlreadyShipped);
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsProductInAnyOrderAsync(int productId)
    {
        var isInAnyOrder = await _context.OrderItems.AnyAsync(item => item.ProductId == productId);

        // Retorna false si el producto está en alguna orden (no se puede eliminar)
        // Retorna true si el producto NO está en ninguna orden (se puede eliminar)
        return !isInAnyOrder;
    }

    public async Task<PaginatedResult<Order>> GetOrdersPaginatedAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.Orders.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var orders = await _context
            .Orders.Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<Order>
        {
            Items = orders,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }
}
