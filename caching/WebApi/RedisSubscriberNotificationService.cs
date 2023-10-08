using StackExchange.Redis;

public interface IRedisSubscriber
{
    Task OnItemRemoved(string key);
}


public class RedisSubscriber : IRedisSubscriber
{
    readonly InMemoryGenericCache inMemoryGenericCache;
    readonly ILogger<RedisSubscriber> logger;
    public RedisSubscriber(InMemoryGenericCache inMemoryGenericCache, ILogger<RedisSubscriber> logger)
    {
        this.logger = logger;
        this.inMemoryGenericCache = inMemoryGenericCache;
    }
    public async Task OnItemRemoved(string key)
    {
        logger.LogInformation($"redis key deleted {key}");
        await inMemoryGenericCache.RemoveItem(key);
    }
}

public class RedisSubscriberNotificationService : BackgroundService
{
    readonly IEnumerable<IRedisSubscriber> subscribers;
    readonly ConnectionMultiplexer redis;
    public RedisSubscriberNotificationService(IEnumerable<IRedisSubscriber> subscribers, ConnectionMultiplexer redis)
    {
        this.redis = redis;
        this.subscribers = subscribers;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {

        if (subscribers.Any())
        {
            var sub = redis.GetSubscriber();
            sub.Subscribe("__keyevent@0__:del", async (c, m) =>
            {
                await Task.WhenAll(subscribers.Select(async subscriber => await subscriber.OnItemRemoved(m)));
            });
        }
    }
}