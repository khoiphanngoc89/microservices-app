﻿namespace Basket.Api.Domain;

public class ShoppingCart
{
    public string UserName { get; set; } = default!;
    public List<ShoppingCartItem> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

    public ShoppingCart(string userName)
    {
        this.UserName = userName;
    }

    // Required for mapping
    public ShoppingCart()
    {
    }
}
