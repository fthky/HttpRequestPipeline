using HttpRequestPipeline.Client.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .UseContentRoot(AppContext.BaseDirectory)
    .ConfigureServices((context, services) =>
    {
        var gatewayBaseUrl = context.Configuration["Gateway:BaseUrl"];
        if (string.IsNullOrWhiteSpace(gatewayBaseUrl))
            throw new InvalidOperationException(
                "Gateway:BaseUrl is required.");

        services.AddTransient<CorrelationIdHandler>();
        services.AddTransient<ClientLoggingHandler>();

        services.AddHttpClient("gateway", client =>
            {
                client.BaseAddress = new Uri(gatewayBaseUrl);
            })
            .AddHttpMessageHandler<CorrelationIdHandler>()
            .AddHttpMessageHandler<ClientLoggingHandler>();
    })
    .Build();

var factory = host.Services.GetRequiredService<IHttpClientFactory>();
var http = factory.CreateClient("gateway");

var res = await http.GetAsync("/ping");
res.EnsureSuccessStatusCode();

var body = await res.Content.ReadAsStringAsync();

Console.WriteLine($"BODY: {body}");
