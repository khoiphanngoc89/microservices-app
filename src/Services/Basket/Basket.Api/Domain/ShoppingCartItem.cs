namespace Basket.Api.Domain;

public class ShoppingCartItem
{
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;
}
