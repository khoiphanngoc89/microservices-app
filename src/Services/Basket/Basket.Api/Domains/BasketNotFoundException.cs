namespace Basket.Api.Domains;

public sealed class BasketNotFoundException
    : NotFoundException
{

    public BasketNotFoundException(string userName)
        : base("Basket", userName)
    {
    }

    public static void ThrowIfNull([NotNull] ShoppingCart? cart, [CallerArgumentExpression(nameof(cart))] string userName = default!)
    {
        if (cart is null)
        {
            throw new BasketNotFoundException(userName);
        }
    }
}
