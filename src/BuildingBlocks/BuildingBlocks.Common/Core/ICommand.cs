using MediatR;

namespace BuildingBlocks.Common.Core;

public interface ICommand : ICommand<Unit>
{
} 

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
