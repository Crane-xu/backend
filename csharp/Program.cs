
using MinAPISeparateFile;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAppServices(builder.Configuration);

var app = builder.Build();
app.UseMiddleware();
TodoEndpoints.Map(app);
app.Run();
