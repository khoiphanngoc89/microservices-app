using MediatR;

namespace BuildingBlocks.Cores;

public interface ICommand : ICommand<Unit>
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}