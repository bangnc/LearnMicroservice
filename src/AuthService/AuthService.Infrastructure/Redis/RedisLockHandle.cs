using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Redis
{
    internal sealed class RedisLockHandle : IAsyncDisposable
    {
        private readonly IDatabase _database;
        private readonly string _resource;
        private readonly string _token;

        private const string ReleaseScript = """
        if redis.call('GET', KEYS[1]) == ARGV[1] then
            return redis.call('DEL', KEYS[1])
        else
            return 0
        end
        """;

        public RedisLockHandle(
            IDatabase database,
            string resource,
            string token)
        {
            _database = database;
            _resource = resource;
            _token = token;
        }

        public async ValueTask DisposeAsync()
        {
            await _database.ScriptEvaluateAsync(
                ReleaseScript,
                new RedisKey[] { _resource },
                new RedisValue[] { _token });
        }
    }
}
