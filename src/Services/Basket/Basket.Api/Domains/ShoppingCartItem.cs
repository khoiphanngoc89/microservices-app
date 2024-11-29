namespace Basket.Api.Domains;

public class ShoppingCartItem
{
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Color { get; set; } = default!;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;
}
