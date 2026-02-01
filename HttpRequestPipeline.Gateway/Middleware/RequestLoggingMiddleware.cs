namespace HttpRequestPipeline.Gateway.Middleware;

public sealed class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/health"))
        {
            await _next(context);
            return;
        }

        var correlationId = context.Items["X-Request-Id"]?.ToString() ?? "n/a";

        _logger.LogInformation("IN -> Method: {RequestMethod}, Path: {RequestPath} | X-Request-Id: {CorrelationId}",
            context.Request.Method,
            context.Request.Path,
            correlationId);

        await _next(context);

        _logger.LogInformation("OUT -> StatusCode: {ResponseStatusCode}, Method: {RequestMethod}, Path: {RequestPath} | X-Request-Id: {CorrelationId}",
            context.Response.StatusCode,
            context.Request.Method,
            context.Request.Path,
            correlationId);
    }
}