namespace WebHost.Services;

public interface IAuctionBackgroundService
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}