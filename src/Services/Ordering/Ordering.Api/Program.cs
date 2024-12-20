using Ordering.Api;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.AddServies().UsePipeline();






app.Run();
