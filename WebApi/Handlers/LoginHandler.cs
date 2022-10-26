using System.Text.Json;

namespace WebApi.Handlers;

public class LoginHandler : DelegatingHandler
{
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(ILogger<LoginHandler> logger)
    {
        _logger = logger;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@$"REQUEST:
        TO:{request.RequestUri}
        Method:{request.Method}
        Headers:`n {JsonSerializer.Serialize(request.Headers)}
        Body:{JsonSerializer.Serialize(request.Content)}");

        return await base.SendAsync(request, cancellationToken);
    }
}