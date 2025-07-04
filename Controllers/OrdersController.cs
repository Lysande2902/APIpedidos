using APIPedidos.DTOs;
using APIPedidos.Models;
using APIPedidos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIPedidos.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IValidationService _validationService;

    public OrdersController(IOrderService orderService, IValidationService validationService)
    {
        _orderService = orderService;
        _validationService = validationService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<OrderResponseDto>>>> GetOrders()
    {
        try
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var orderDtos = orders.Select(MapToOrderResponseDto);
            return Ok(
                ApiResponse<IEnumerable<OrderResponseDto>>.SuccessResponse(
                    orderDtos,
                    "Órdenes obtenidas exitosamente"
                )
            );
        }
        catch (Exception ex)
        {
            // Log detallado para depuración
            Console.WriteLine($"[ERROR] GET /api/orders: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(
                500,
                ApiResponse<IEnumerable<OrderResponseDto>>.ErrorResponse(
                    $"{ValidationMessages.ServerError} - {ex.Message}",
                    new List<string> { ex.StackTrace ?? "" }
                )
            );
        }
    }

    [HttpGet("paginated")]
    public async Task<
        ActionResult<ApiResponse<PaginatedResult<OrderResponseDto>>>
    > GetOrdersPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        // Validar parámetros de paginación
        var (isValid, errors) = _validationService.ValidatePagination(pageNumber, pageSize);
        if (!isValid)
        {
            return BadRequest(
                ApiResponse<PaginatedResult<OrderResponseDto>>.ErrorResponse(
                    ValidationMessages.InvalidRequest,
                    errors
                )
            );
        }

        try
        {
            var result = await _orderService.GetOrdersPaginatedAsync(pageNumber, pageSize);
            var orderDtos = result.Items.Select(MapToOrderResponseDto).ToList();

            var paginatedResult = new PaginatedResult<OrderResponseDto>
            {
                Items = orderDtos,
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,
            };

            return Ok(
                ApiResponse<PaginatedResult<OrderResponseDto>>.SuccessResponse(
                    paginatedResult,
                    "Órdenes paginadas obtenidas exitosamente"
                )
            );
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                ApiResponse<PaginatedResult<OrderResponseDto>>.ErrorResponse(
                    ValidationMessages.ServerError
                )
            );
        }
    }

    [HttpGet("{orderId}")]
    public async Task<ActionResult<ApiResponse<OrderResponseDto>>> GetOrder(int orderId)
    {
        // Validar ID de la orden
        if (orderId <= 0)
        {
            return BadRequest(
                ApiResponse<OrderResponseDto>.ErrorResponse(ValidationMessages.OrderIdInvalid)
            );
        }

        try
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                return NotFound(
                    ApiResponse<OrderResponseDto>.ErrorResponse(ValidationMessages.OrderNotFound)
                );
            }

            return Ok(
                ApiResponse<OrderResponseDto>.SuccessResponse(
                    MapToOrderResponseDto(order),
                    "Orden obtenida exitosamente"
                )
            );
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                ApiResponse<OrderResponseDto>.ErrorResponse(ValidationMessages.ServerError)
            );
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<OrderResponseDto>>> CreateOrder()
    {
        try
        {
            var order = await _orderService.CreateOrderAsync();
            return CreatedAtAction(
                nameof(GetOrder),
                new { orderId = order.Id },
                ApiResponse<OrderResponseDto>.SuccessResponse(
                    MapToOrderResponseDto(order),
                    "Orden creada exitosamente"
                )
            );
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                ApiResponse<OrderResponseDto>.ErrorResponse(ValidationMessages.ServerError)
            );
        }
    }

    [HttpPost("{orderId}/items")]
    public async Task<ActionResult<ApiResponse<OrderItemResponseDto>>> AddItemToOrder(
        int orderId,
        [FromBody] AddOrderItemDto addItemDto
    )
    {
        // Validar ID de la orden
        if (orderId <= 0)
        {
            return BadRequest(
                ApiResponse<OrderItemResponseDto>.ErrorResponse(ValidationMessages.OrderIdInvalid)
            );
        }

        // Validar ítem de orden
        var (isValid, errors) = _validationService.ValidateOrderItem(
            addItemDto.ProductId,
            addItemDto.Quantity
        );
        if (!isValid)
        {
            return BadRequest(
                ApiResponse<OrderItemResponseDto>.ErrorResponse(
                    ValidationMessages.InvalidRequest,
                    errors
                )
            );
        }

        try
        {
            var orderItem = await _orderService.AddItemToOrderAsync(
                orderId,
                addItemDto.ProductId,
                addItemDto.Quantity
            );
            return Ok(
                ApiResponse<OrderItemResponseDto>.SuccessResponse(
                    MapToOrderItemResponseDto(orderItem),
                    "Ítem agregado a la orden exitosamente"
                )
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<OrderItemResponseDto>.ErrorResponse(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<OrderItemResponseDto>.ErrorResponse(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                ApiResponse<OrderItemResponseDto>.ErrorResponse(ValidationMessages.ServerError)
            );
        }
    }

    [HttpPut("{orderId}/state")]
    public async Task<ActionResult<ApiResponse>> UpdateOrderState(
        int orderId,
        [FromBody] UpdateOrderStateDto updateStateDto
    )
    {
        // Validar ID de la orden
        if (orderId <= 0)
        {
            return BadRequest(ApiResponse.ErrorResponse(ValidationMessages.OrderIdInvalid));
        }

        // Validar estado
        var (isValid, errors) = _validationService.ValidateOrderState(updateStateDto.State);
        if (!isValid)
        {
            return BadRequest(ApiResponse.ErrorResponse(ValidationMessages.InvalidRequest, errors));
        }

        try
        {
            if (!Enum.TryParse<OrderState>(updateStateDto.State, true, out var newState))
            {
                return BadRequest(ApiResponse.ErrorResponse(ValidationMessages.OrderStateInvalid));
            }

            var success = await _orderService.UpdateOrderStateAsync(orderId, newState);

            if (!success)
            {
                return NotFound(ApiResponse.ErrorResponse(ValidationMessages.OrderNotFound));
            }

            return Ok(ApiResponse.SuccessResponse("Estado de la orden actualizado exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.ErrorResponse(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse.ErrorResponse(ValidationMessages.ServerError));
        }
    }

    [HttpDelete("{orderId}")]
    public async Task<ActionResult<ApiResponse>> DeleteOrder(int orderId)
    {
        // Validar ID de la orden
        if (orderId <= 0)
        {
            return BadRequest(ApiResponse.ErrorResponse(ValidationMessages.OrderIdInvalid));
        }

        try
        {
            var success = await _orderService.DeleteOrderAsync(orderId);

            if (!success)
            {
                return NotFound(ApiResponse.ErrorResponse(ValidationMessages.OrderNotFound));
            }

            return Ok(ApiResponse.SuccessResponse("Orden eliminada exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.ErrorResponse(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse.ErrorResponse(ValidationMessages.ServerError));
        }
    }

    private static OrderResponseDto MapToOrderResponseDto(Order order)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            CreatedAt = order.CreatedAt,
            State = order.State.ToString(),
            Total = order.Total,
            Items = (order.Items ?? new List<OrderItem>())
                .Select(MapToOrderItemResponseDto)
                .ToList(),
        };
    }

    private static OrderItemResponseDto MapToOrderItemResponseDto(OrderItem orderItem)
    {
        return new OrderItemResponseDto
        {
            Id = orderItem.Id,
            ProductId = orderItem.ProductId,
            ProductName =
                orderItem.Product != null ? orderItem.Product.Name : "Producto no disponible",
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice,
            Subtotal = orderItem.Subtotal,
        };
    }
}
