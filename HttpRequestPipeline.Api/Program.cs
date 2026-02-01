var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseMiddleware<HttpRequestPipeline.Api.Middleware.CorrelationIdMiddleware>();
app.UseMiddleware<HttpRequestPipeline.Api.Middleware.RequestLoggingMiddleware>();

app.MapGet("/ping", () => "Pong");

app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "api" }));

app.Run();
