using BuildingBlocks.Common.Logging;
using Catalog.Api;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// register for serilog
builder.Host.UseSerilog(Serilogger.Configure);
var app = builder.ConfigureServices().ConfigurePipeline();

app.Run();
