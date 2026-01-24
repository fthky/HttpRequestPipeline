var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseMiddleware<HttpRequestPipeline.Api.Middleware.CorrelationIdMiddleware>();
app.UseMiddleware<HttpRequestPipeline.Api.Middleware.RequestLoggingMiddleware>();

app.MapGet("/ping", () => "Pong");

app.Run();
