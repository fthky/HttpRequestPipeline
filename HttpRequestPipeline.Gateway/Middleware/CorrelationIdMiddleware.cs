namespace HttpRequestPipeline.Gateway.Middleware;

public sealed class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    internal const string HeaderName = "X-Request-Id";
    
    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var correlationId = context.Request.Headers[HeaderName].FirstOrDefault();

        if (String.IsNullOrWhiteSpace(correlationId))
            correlationId = Guid.NewGuid().ToString("N");

        context.Items[HeaderName] = correlationId;
        context.Response.Headers[HeaderName] = correlationId;

        await _next(context);
    }
}