using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace Infrastructure.Test.Caching;

public class LocalCacheService : CacheService<API.Starter.Infrastructure.Caching.LocalCacheService>
{
    protected override API.Starter.Infrastructure.Caching.LocalCacheService CreateCacheService() =>
        new(
            new MemoryCache(new MemoryCacheOptions()),
            NullLogger<API.Starter.Infrastructure.Caching.LocalCacheService>.Instance);
}