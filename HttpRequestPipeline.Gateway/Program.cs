var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("api", (serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    client.BaseAddress = new Uri(configuration["Api:BaseUrl"]!);
});
var app = builder.Build();

app.UseMiddleware<HttpRequestPipeline.Gateway.Middleware.CorrelationIdMiddleware>();
app.UseMiddleware<HttpRequestPipeline.Gateway.Middleware.RequestLoggingMiddleware>();


app.MapGet("/ping", async (HttpContext context, IHttpClientFactory httpClientFactory) =>
{
    
    var correlationId = context.Items[
        HttpRequestPipeline.Gateway.Middleware.CorrelationIdMiddleware.HeaderName
    ]?.ToString();

    var client = httpClientFactory.CreateClient("api");

    using var req = new HttpRequestMessage(HttpMethod.Get, "/ping");
    req.Headers.TryAddWithoutValidation("X-Request-Id", correlationId);

    var res = await client.SendAsync(req);

    var body = await res.Content.ReadAsStringAsync();

    return Results.Content(body, "text/plain");
});

app.Run();
