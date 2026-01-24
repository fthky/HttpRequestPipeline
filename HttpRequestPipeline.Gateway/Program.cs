var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("api", (serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    client.BaseAddress = new Uri(configuration["Api:BaseUrl"]!);
});
var app = builder.Build();

app.MapGet("/ping", async (HttpContext context, IHttpClientFactory httpClientFactory) =>
{
    var correlationId = context.Request.Headers["X-Request-Id"].FirstOrDefault();
    if (string.IsNullOrWhiteSpace(correlationId))
    {
        correlationId = Guid.NewGuid().ToString("N");
    }
    
    context.Response.Headers["X-Request-Id"] = correlationId;

    var client = httpClientFactory.CreateClient("api");

    using var req = new HttpRequestMessage(HttpMethod.Get, "/ping");
    req.Headers.TryAddWithoutValidation("X-Request-Id", correlationId);

    var res = await client.SendAsync(req);

    var body = await res.Content.ReadAsStringAsync();

    return Results.Content(body, "text/plain");
});

app.Run();
