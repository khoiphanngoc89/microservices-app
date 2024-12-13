using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Api.Infrastructure;

public sealed class CachedBasketRepository
    (IBasketRepository repository, IDistributedCache cached)
    : IBasketRepository
{
    public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await cached.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrWhiteSpace(cachedBasket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        }    
            
        var basket = await repository.GetBasketAsync(userName, cancellationToken);
        await cached.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
        return basket;
    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.StoreBasketAsync(basket, cancellationToken);
        await cached.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket),cancellationToken);
        return basket;
    }

    public async Task<bool> DeleteBasketAsnyc(string userName, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBasketAsnyc(userName, cancellationToken);
        await cached.RemoveAsync(userName, cancellationToken);
        return true;
    }
}
