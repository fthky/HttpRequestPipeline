namespace Lab.Api.Middleware;

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
        _logger.LogInformation($"IN -> Method: {context.Request.Method}, Path: {context.Request.Path}");

        await _next(context);
        
        _logger.LogInformation($"OUT -> StatusCode: {context.Response.StatusCode}, Method: {context.Request.Method}, Path: {context.Request.Path}");
    }
}