using Microsoft.Extensions.Logging;

namespace HttpRequestPipeline.Client.Http;

public sealed class ClientLoggingHandler : DelegatingHandler
{

    private readonly ILogger<ClientLoggingHandler> _logger;
    private const string HeaderName = "X-Request-Id";
    
    public ClientLoggingHandler(ILogger<ClientLoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var correlationId = request.Headers.TryGetValues(HeaderName, out var values)
            ? values.FirstOrDefault()
            : "n/a";

        _logger.LogInformation($"CLIENT -> {request.Method} {request.RequestUri} | X-Request-Id = {correlationId}");

        var response = await base.SendAsync(request, cancellationToken);

        var returnedCorrelationId = response.Headers.TryGetValues(HeaderName, out var outValues) 
            ? outValues.FirstOrDefault() 
            : "n/a";

        _logger.LogInformation($"CLIENT <- {(int)response.StatusCode} {response.ReasonPhrase} {request.RequestUri} | X-Request-Id = {returnedCorrelationId}");
        
        return response;
    }
}