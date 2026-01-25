namespace HttpRequestPipeline.Client.Http;

public sealed class CorrelationIdHandler : DelegatingHandler
{
    private const string HeaderName = "X-Request-Id";

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Contains(HeaderName)) return base.SendAsync(request, cancellationToken);
        
        var correlationId = Guid.NewGuid().ToString("N");
        request.Headers.TryAddWithoutValidation(HeaderName, correlationId);

        return base.SendAsync(request, cancellationToken);
    }
}