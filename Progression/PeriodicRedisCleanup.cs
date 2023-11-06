using StackExchange.Redis;

namespace WeatherApp.Services;

public class PeriodicRedisCleanup : BackgroundService
{
	private readonly IConnectionMultiplexer _redisConnection;
	private readonly Logger<PeriodicRedisCleanup> _logger;

	public PeriodicRedisCleanup(IConnectionMultiplexer redisConnection, Logger<PeriodicRedisCleanup> logger)
	{
		_redisConnection = redisConnection;
		_logger = logger;
		
	}

	private readonly TimeSpan CleanupInterval = TimeSpan.FromMinutes(15);
	public bool IsEnabled { get; set; } = false;
	
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using var timer = new PeriodicTimer(CleanupInterval);
		while (!stoppingToken.IsCancellationRequested &&
		       await timer.WaitForNextTickAsync(stoppingToken))
		{
			try
			{
				if (IsEnabled)
				{
					await CleanupRedis();
					_logger.LogInformation("Redis cleanup done");
				}
				else
				{
					_logger.LogInformation("Redis cleanup skipped");
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error while cleaning up redis");
			}
		}
	}

	private async Task CleanupRedis()
	{
	}
}