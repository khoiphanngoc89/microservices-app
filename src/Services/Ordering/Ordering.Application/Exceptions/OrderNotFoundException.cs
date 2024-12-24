using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using BuildingBlocks.Domains;

namespace Ordering.Application.Exceptions;

public sealed class OrderNotFoundException : NotFoundException
{
    private const string Order = nameof(Order);
    public OrderNotFoundException(string message) : base(message)
    {
    }

    public OrderNotFoundException(Guid id) : base(Order, id)
    {
    }

    public OrderNotFoundException(Guid id, Exception? innerException) : base(Order, id, innerException)
    {
    }

    public static void ThrowIfNull([NotNull] Order? order, [CallerArgumentExpression(nameof(order))] string paramName = default!)
    {
        if (order is null)
        {
            throw new OrderNotFoundException(paramName);
        }
    }

    public static void ThrowIfNull([NotNull] Order? order, OrderId orderId)
    {
        if (order is null)
        {
            throw new OrderNotFoundException(orderId!.Value);
        }
    }
}
