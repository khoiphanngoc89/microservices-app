using MediatR;

namespace BuildingBlocks.Common.Core;


public interface IQuery<out TReponse> : IRequest<TReponse>
    where TReponse : notnull
{
}
