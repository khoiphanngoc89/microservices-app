using System.Linq.Expressions;
using System.Reflection;
using BuildingBlocks.Domains;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BuildingBlocks.Application.Endpoints;

/// <summary>
/// https://www.milanjovanovic.tech/blog/automatically-register-minimal-apis-in-aspnetcore
/// </summary>
public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var endpoints = assembly!.DefinedTypes
           .Where(type => type is { IsAbstract: false, IsInterface: false } &&
           type.IsAssignableTo(typeof(IEndpointModule)))
           .Select(type => ServiceDescriptor.Transient(typeof(IEndpointModule), type))
           .ToArray();

        services.TryAddEnumerable(endpoints);
        return services;
    }

    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpointModule> endpoints = app.Services
        .GetRequiredService<IEnumerable<IEndpointModule>>();

        IEndpointRouteBuilder builder =
            routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpointModule endpoint in endpoints)
        {
            endpoint.AddEndpoints(builder);
        }

        return app;
    }
}
