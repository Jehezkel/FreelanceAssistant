namespace WebApi;
public class RefreshManager : IHostedService, IDisposable
{
    private readonly ILogger<RefreshManager> _logger;
    private readonly FreelancerClient _client;
    private Timer? _timer = null;

    public RefreshManager(ILogger<RefreshManager> logger, FreelancerClient client)
    {
        this._logger = logger;
        this._client = client;
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

    private void DoWork(object? state)
    {
        _logger.LogInformation("Triggered");
        if (_client.IsAuthorized)
        {
            _logger.LogInformation("Work, work");
            _client.fetchProjects();

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
