using HttpRequestPipeline.Client.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .UseContentRoot(AppContext.BaseDirectory)
    .ConfigureAppConfiguration(config =>
    {
        config.SetBasePath(AppContext.BaseDirectory);
        config.AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();
    }).ConfigureServices((context, services) =>
    {
        var gatewayBaseUrl = context.Configuration["Gateway:BaseUrl"];
        if (string.IsNullOrWhiteSpace(gatewayBaseUrl))
            throw new InvalidOperationException(
                "Gateway:BaseUrl is required.");

        services.AddTransient<CorrelationIdHandler>();

        services.AddHttpClient("gateway", client =>
            {
                client.BaseAddress = new Uri(gatewayBaseUrl);
            })
            .AddHttpMessageHandler<CorrelationIdHandler>();
    })
    .Build();

var factory = host.Services.GetRequiredService<IHttpClientFactory>();
var http = factory.CreateClient("gateway");

Console.WriteLine("CLIENT -> GET /ping (X-Request-Id is added automatically)");
var res = await http.GetAsync("/ping");

var body = await res.Content.ReadAsStringAsync();
var returnedRequestId = res.Headers.TryGetValues("X-Request-Id", out var values) ? values.FirstOrDefault() : null;

Console.WriteLine($"CLIENT <- {(int)res.StatusCode} {res.ReasonPhrase} | X-Request-Id={returnedRequestId}");
Console.WriteLine($"BODY: {body}");