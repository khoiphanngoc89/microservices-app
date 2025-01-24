using Basket.Api.Entities;
using BuildingBlocks.Common.Core.Exceptions;

namespace Basket.Api.Shared;

public sealed class BasketNotFoundException(string userName)
        : NotFoundException(Basket, userName)
{
    private const string Basket = nameof(Basket);

    public static void ThrowIfNull(ShoppingCart? basket, string userName)
    {
        if (basket is null)
        {
            throw new BasketNotFoundException(userName);
        }
    }
}
