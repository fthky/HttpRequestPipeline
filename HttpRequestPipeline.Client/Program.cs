using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();
    
var gatewayBaseUrl = config["Gateway:BaseUrl"];

if (String.IsNullOrWhiteSpace(gatewayBaseUrl))
{
    throw new InvalidOperationException("Gateway:BaseUrl is required");
}

using var http = new HttpClient();
http.BaseAddress = new Uri(gatewayBaseUrl);

var correlationId = "test-123"; //Guid.NewGuid().ToString("N");

using var request = new HttpRequestMessage(HttpMethod.Get, "/ping");
request.Headers.TryAddWithoutValidation("X-Request-Id", correlationId);

Console.WriteLine($"CLIENT -> GET /ping | X-Request-Id={correlationId}");

using var response = await http.SendAsync(request);

var responseBody = await response.Content.ReadAsStringAsync();
var returnedCorrelationId = response.Headers.TryGetValues("X-Request-Id", out var values)
    ? values.FirstOrDefault()
    : null;

Console.WriteLine($"CLIENT <- {(int)response.StatusCode} {response.ReasonPhrase} | X-Request-Id={returnedCorrelationId}");
Console.WriteLine($"BODY: {responseBody}");
