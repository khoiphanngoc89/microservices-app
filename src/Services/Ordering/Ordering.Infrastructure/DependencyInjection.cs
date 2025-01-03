﻿using BuildingBlocks.Domains;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string conectionStrings)
    {
        services.AddDatabase(conectionStrings);

        //services.AddScoped<IOrderRepository, OrderRepository>();
        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, string connectionStrings)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddDbContext<OrderingDbContext>((sp, options) =>
        {
            // add all audit interceptors
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionStrings);
        });

        // register abstract module that will be used by Ordering.Application
        services.AddScoped<IOrderingDbContext, OrderingDbContext>();
        return services;
    }
}
