using Basket.Api.Database;
using BuildingBlocks.Common.Core;

namespace Basket.Api.Features.Basket.DeleteBasket;

public sealed record DeleteBasketCommand(string UserName)
    : ICommand<DeleteBasketResult>;
public sealed record DeleteBasketResult(bool IsSucceed);

public class DeleteBasketCommandHandler
    (IBasketRepository repository)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        await repository.DeleteBasket(command.UserName, cancellationToken);
        return new (true);
    }
}
