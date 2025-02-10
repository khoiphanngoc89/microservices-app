using Common.Logging;
using Discount.Grpc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);
var app = builder.ConfigureServices().ConfigurePipeline();

app.Run();

// for testing on postman,
// to use https of grpc 
// you need to turn on the TLS
// (lock icon)
