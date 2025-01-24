using Basket.Api;
using BuildingBlocks.Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// register for serilog
builder.Host.UseSerilog(Serilogger.Configure);

var app = builder.ConfigureServices().ConfigurePipeline();

app.Run();
