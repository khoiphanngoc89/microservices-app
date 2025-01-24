using MediatR;
using System.Diagnostics;
using ILogger = Serilog.ILogger;

namespace BuildingBlocks.Common.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>
    (ILogger logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.Information("[START] Handling request={Request} - Response={Response} - RequestData={RequestData}",
            typeof(TRequest).Name,
            typeof(TResponse).Name,
            request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();
        var time = timer.Elapsed;
        if (time.Seconds > 3)
        {
            logger.Warning("[PERFORMANCE] Handling request={Request} - Response={Response} - ElapsedTime={ElapsedTime}",
                typeof(TRequest).Name,
                typeof(TResponse).Name,
                time.Seconds);
        }

        logger.Information("[END] Handled request={Request} with Response={Response}",
            typeof(TRequest).Name,
            typeof(TResponse).Name);
        return response;
    }
}
