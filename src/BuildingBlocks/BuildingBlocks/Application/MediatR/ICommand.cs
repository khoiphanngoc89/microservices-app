using MediatR;

namespace BuildingBlocks.Application.MediatR;

// Command represents IRequest; however, we wrapper IRquest of MediatR
// to make it more explicit that this is a command
public interface ICommand : ICommand<Unit> // Unit represents void
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}