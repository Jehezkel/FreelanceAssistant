using System.Text.Json;

namespace WebApi.Handlers;

public class RequestLoggingHandler : DelegatingHandler
{
    private readonly ILogger<RequestLoggingHandler> _logger;

    public RequestLoggingHandler(ILogger<RequestLoggingHandler> logger)
    {
        _logger = logger;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var requestFormatted = (@$"REQUEST:
        TO:{request.RequestUri}\n
        Method:{request.Method}\n
        Headers:\n{JsonSerializer.Serialize(request.Headers)}
        Body:{JsonSerializer.Serialize(request.Content)}");

        var result = await base.SendAsync(request, cancellationToken);
        var responseFormatted = @$"RESPONSE:
        CODE:{result.StatusCode}
        BODY:{await result.Content.ReadAsStringAsync()}
        ";
        var responseBody = await result.Content.ReadAsStringAsync();
        var logLevel = LogLevel.Debug;
        if (!result.IsSuccessStatusCode)
        {
            logLevel = LogLevel.Error;
        }
        _logger.Log(logLevel, requestFormatted);
        _logger.Log(logLevel, responseFormatted);

        return result;
    }
}