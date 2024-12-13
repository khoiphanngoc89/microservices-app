// create web application builder to boostraping the application




var builder = WebApplication.CreateBuilder(args);

var app = builder.AddServices().UsePipeline();


// 
app.Run();

