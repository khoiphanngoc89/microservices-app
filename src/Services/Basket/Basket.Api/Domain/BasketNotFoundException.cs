using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Basket.Api.Domain;

public sealed class BasketNotFoundException
    : NotFoundException
{
    private const string Basket = nameof(Basket);
    public BasketNotFoundException(string userName)
        : base(Basket, userName)
    {
    }

    public static void ThrowIfNull([NotNull] ShoppingCart? cart, [CallerArgumentExpression(nameof(cart))] string userName = default!)
    {
        // better readability / Concise
        // == operator can be overloaded and many return a boolean that
        // you don;t expect
        if (cart is null)
        {
            throw new BasketNotFoundException(userName);
        }
    }
}
