namespace Ordering.Infrastructure.Data.Extensions;

internal class InitialData
{
    public static IEnumerable<Customer> Customers =>
    new List<Customer>
    {
        Customer.Create(CustomerId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291aa")), "Evelyn", "evelyn@example.com"),
        Customer.Create(CustomerId.Of(new Guid("189dc8dc-990f-48e0-a37b-e6f2b60b9d7d")), "john", "john@example.com")
    };

    public static IEnumerable<Product> Products =>
       new List<Product>
       {
            Product.Create(ProductId.Of(new Guid("fc01856d-6660-4383-8bce-a8ab4f021c62")), "IPhone X", 950),
            Product.Create(ProductId.Of(new Guid("7b8d3b3e-7d24-4ae4-a9b4-05b482fe2bbf")), "Samsung 10", 750),
            Product.Create(ProductId.Of(new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8")), "Huawei Plus", 650),
            Product.Create(ProductId.Of(new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27")), "Xiaomi Mi", 675)
       };

    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            var address1 = Address.Of("Evelyn", "Harrison", "evelyn@example.com", "Bahcelievler No:4", "+905312345678", "Istanbul", "Turkey", "38050");
            var address2 = Address.Of("John", "Doe", "john@example.com", "Broadway No:1", "+447911123456", "Nottingham", "England", "08050");

            var payment1 = Payment.Of("Evelyn Harrison", "4024007173171975", "7/2027", "906", 1);
            var payment2 = Payment.Of("John Doe", "4024007152722806", "6/2026", "318", 2);

            var order1 = Order.Create(
                            OrderId.Of(Guid.NewGuid()),
                            CustomerId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291aa")),
                            OrderName.Of("ORD_1"),
                            shippingAddress: address1,
                            billingAddress: address1,
                            payment1);
            order1.Add(ProductId.Of(new Guid("7b8d3b3e-7d24-4ae4-a9b4-05b482fe2bbf")), 2, 750);
            order1.Add(ProductId.Of(new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8")), 1, 650);

            var order2 = Order.Create(
                            OrderId.Of(Guid.NewGuid()),
                            CustomerId.Of(new Guid("189dc8dc-990f-48e0-a37b-e6f2b60b9d7d")),
                            OrderName.Of("ORD_2"),
                            shippingAddress: address2,
                            billingAddress: address2,
                            payment2);
            order2.Add(ProductId.Of(new Guid("fc01856d-6660-4383-8bce-a8ab4f021c62")), 1, 950);
            order2.Add(ProductId.Of(new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27")), 2, 675);

            return new List<Order> { order1, order2 };
        }
    }
}
