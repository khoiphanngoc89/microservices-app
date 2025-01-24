using MediatR;

namespace BuildingBlocks.Common.Core;

public interface ICommandHandler<in TCommand>
    : IRequestHandler<TCommand, Unit>
    where TCommand: ICommand<Unit>
{
}    

public interface ICommandHandler<in TCommand, TReponse>
    : IRequestHandler<TCommand, TReponse>
    where TCommand : ICommand<TReponse>
    where TReponse : notnull
{
}
