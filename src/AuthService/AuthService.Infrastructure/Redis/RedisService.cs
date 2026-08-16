using AuthService.Application.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Redis
{
    public class RedisService : IRedisService   
    {
        private readonly IDatabase _database;

        public RedisService(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task SetAsync(
        string key,
        string value,
        TimeSpan? expiry = null)
        {
            await _database.StringSetAsync(
                key,
                value,
                expiry);
        }
        public async Task<string?> GetAsync(string key)
        {
            var value = await _database.StringGetAsync(key);

            return value.IsNull
                ? null
                : value.ToString();
        }

        public async Task DeleteAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }

        public async Task<IAsyncDisposable?> AcquireAsync(
        string resource,
        TimeSpan expiry,
        CancellationToken cancellationToken = default)
        {
            var token = Guid.NewGuid().ToString();

            var acquired = await _database.StringSetAsync(
                resource,
                token,
                expiry,
                When.NotExists);

            if (!acquired)
            {
                return null;
            }

            return new RedisLockHandle(
                _database,
                resource,
                token);
        }
    }
}
