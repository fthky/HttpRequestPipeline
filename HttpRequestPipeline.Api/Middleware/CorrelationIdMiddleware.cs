namespace HttpRequestPipeline.Api.Middleware;
public class CorrelationIdMiddleware
{
   private const string HeaderName = "X-Request-Id";

   private readonly RequestDelegate _next;
   private readonly ILogger<CorrelationIdMiddleware> _logger;

   public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
   {
      _next = next;
      _logger = logger;
   }

   public async Task Invoke(HttpContext context)
   {
      var correlationId = context.Request.Headers[HeaderName].FirstOrDefault();

      if (string.IsNullOrWhiteSpace(correlationId))
      {
         correlationId = Guid.NewGuid().ToString("N");
         _logger.LogInformation($"Generated new {HeaderName}: {correlationId}");
      }
      else
      {
         _logger.LogInformation($"Using incoming {HeaderName}: {correlationId}");
      }

      context.Items[HeaderName] = correlationId;

      context.Response.Headers[HeaderName] = correlationId;

      await _next(context);
   }
}