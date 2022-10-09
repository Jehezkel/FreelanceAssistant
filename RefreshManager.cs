using WebApi.Services;

namespace WebApi;
public class RefreshManager : IHostedService, IDisposable
{
    private readonly ILogger<RefreshManager> _logger;
    private readonly FreelancerClient _client;
    private readonly MailService _mailService;
    private Timer? _timer = null;

    public RefreshManager(ILogger<RefreshManager> logger, FreelancerClient client, MailService mailService)
    {
        this._logger = logger;
        this._client = client;
        _mailService = mailService;
    }
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // throw new NotImplementedException();
        _logger.LogInformation("Starting!");
        _timer = new Timer(DoWork, null, 0, 30_000);
        return Task.CompletedTask;

    }

    private async void DoWork(object? state)
    {
        _logger.LogInformation("Triggered");
        if (_client.IsAuthorized)
        {
            _logger.LogInformation("Work, work");
            await _client.fetchProjects();
            await _mailService.SendEmailAsync();

        }
        else
        {
            _logger.LogWarning("Not authorized - no work");
        }
    }




    public Task StopAsync(CancellationToken cancellationToken)
    {
        // throw new NotImplementedException();
        _logger.LogInformation("Stopping!");
        return Task.CompletedTask;
    }
}
