namespace APIPedidos.DTOs;

public class OrderResponseDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string State { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public List<OrderItemResponseDto> Items { get; set; } = new();
}

public class UpdateOrderStateDto
{
    public string State { get; set; } = string.Empty;
}
