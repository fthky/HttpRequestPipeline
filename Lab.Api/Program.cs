var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseMiddleware<Lab.Api.Middleware.RequestLoggingMiddleware>();

app.MapGet("/ping", () => "Pong");

app.Run();
