using Microsoft.AspNetCore.Routing;

namespace BuildingBlocks.Application.Endpoints;

public interface IEndpointModule
{
    void AddEndpoints(IEndpointRouteBuilder builder);
}
