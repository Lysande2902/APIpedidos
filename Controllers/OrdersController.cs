using APIPedidos.DTOs;
using APIPedidos.Models;
using APIPedidos.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIPedidos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        var orderDtos = orders.Select(MapToOrderResponseDto);
        return Ok(orderDtos);
    }

    [HttpGet("{orderId}")]
    public async Task<ActionResult<OrderResponseDto>> GetOrder(int orderId)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);

        if (order == null)
            return NotFound();

        return Ok(MapToOrderResponseDto(order));
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponseDto>> CreateOrder()
    {
        var order = await _orderService.CreateOrderAsync();
        return CreatedAtAction(
            nameof(GetOrder),
            new { orderId = order.Id },
            MapToOrderResponseDto(order)
        );
    }

    [HttpPost("{orderId}/items")]
    public async Task<ActionResult<OrderItemResponseDto>> AddItemToOrder(
        int orderId,
        [FromBody] AddOrderItemDto addItemDto
    )
    {
        try
        {
            var orderItem = await _orderService.AddItemToOrderAsync(
                orderId,
                addItemDto.ProductId,
                addItemDto.Quantity
            );
            return Ok(MapToOrderItemResponseDto(orderItem));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{orderId}/state")]
    public async Task<ActionResult> UpdateOrderState(
        int orderId,
        [FromBody] UpdateOrderStateDto updateStateDto
    )
    {
        if (!Enum.TryParse<OrderState>(updateStateDto.State, true, out var newState))
        {
            return BadRequest(
                new { message = "Invalid state. Valid states are: Pendiente, Pagado, Enviado" }
            );
        }

        var success = await _orderService.UpdateOrderStateAsync(orderId, newState);

        if (!success)
            return NotFound();

        return NoContent();
    }

    private static OrderResponseDto MapToOrderResponseDto(Order order)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            CreatedAt = order.CreatedAt,
            State = order.State.ToString(),
            Total = order.Total,
            Items = order.Items.Select(MapToOrderItemResponseDto).ToList(),
        };
    }

    private static OrderItemResponseDto MapToOrderItemResponseDto(OrderItem orderItem)
    {
        return new OrderItemResponseDto
        {
            Id = orderItem.Id,
            ProductId = orderItem.ProductId,
            ProductName = orderItem.Product.Name,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice,
            Subtotal = orderItem.Subtotal,
        };
    }
}
