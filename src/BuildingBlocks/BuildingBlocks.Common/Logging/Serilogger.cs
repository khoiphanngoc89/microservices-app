using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Common.Logging;

public static class Serilogger
{
    public static Action<HostBuilderContext, LoggerConfiguration> Configure => (cxt, conf) =>
    {
        var appplicationName = cxt.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
        var environmentName = cxt.HostingEnvironment.EnvironmentName ?? "Development";

        conf.WriteTo.Debug()
            .WriteTo.Console(outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Environment", environmentName)
            .Enrich.WithProperty("Application", appplicationName)
            .ReadFrom.Configuration(cxt.Configuration);
    };
}
