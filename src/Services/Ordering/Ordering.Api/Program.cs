using Ordering.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.AddServies().UsePipeline();

//if (app.Environment.IsDevelopment())
//{
//    await app.InitlialiseDatabase();
//}

app.Run();
