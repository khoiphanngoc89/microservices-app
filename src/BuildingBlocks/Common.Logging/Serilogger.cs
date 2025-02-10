using Microsoft.Extensions.Hosting;
using Serilog;

namespace Common.Logging;

public static class Serilogger
{
    public static Action<HostBuilderContext, LoggerConfiguration> Configure => (ctx, cfn) =>
    {
        var appName = ctx.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
        var envName = ctx.HostingEnvironment.EnvironmentName ?? "Development";
        // https://github.com/serilog/serilog/wiki/Formatting-Output
        cfn.WriteTo.Debug()
        .WriteTo.Console(outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithProperty("Environment", envName)
        .Enrich.WithProperty("Application", appName)
        .ReadFrom.Configuration(ctx.Configuration);
    };
}
