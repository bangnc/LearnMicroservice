using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface IRedisService
    {
        Task SetAsync(string key, string value, TimeSpan? expiry = null);
        Task<string?> GetAsync(string key);
        Task DeleteAsync(string key);
        Task<IAsyncDisposable?> AcquireAsync(
        string resource,
        TimeSpan expiry,
        CancellationToken cancellationToken = default);
    }
}
