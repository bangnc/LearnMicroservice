using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Interfaces
{
    public interface ILoginSessionRepository
    {
        Task AddAsync(LoginSession session, CancellationToken cancellationToken);

        Task<LoginSession?> GetBySessionIdAsync(
            string sessionId,
            CancellationToken cancellationToken);

        Task UpdateAsync(LoginSession session, CancellationToken cancellationToken);
    }
}
