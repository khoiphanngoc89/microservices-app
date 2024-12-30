using BuildingBlocks.Messaging.Events;

namespace Basket.Api.Application.Transformers;

public sealed class BasketCheckoutTransformer
{
    private BasketCheckoutEvent _basketCheckout;
    private IEnumerable<BasketItemCheckout> _items;
    private decimal _totalPrice;

    private BasketCheckoutTransformer()
    {
    }

    public static BasketCheckoutTransformer Init() => new();

    public BasketCheckoutTransformer DataSource(BasketCheckoutEvent source)
    {
        _basketCheckout = source;
        return this;
    }

    public BasketCheckoutTransformer WithItems(IEnumerable<ShoppingCartItem> items)
    {
        _items = items.Select(x =>
        {
            return new BasketItemCheckout
            {
                Price = x.Price,
                ProductId = x.ProductId,
                Quantity = x.Quantity
            };
        });
        return this;
    }
    
    public BasketCheckoutTransformer WithTotalPrice(decimal totalPrice)
    {
        _totalPrice = totalPrice;
        return this;
    }

    public BasketCheckoutEvent Transform()
    {
        _basketCheckout.Items = new List<BasketItemCheckout>(_items);
        _basketCheckout.TotalPrice = _totalPrice;
        return _basketCheckout;
    }
}
