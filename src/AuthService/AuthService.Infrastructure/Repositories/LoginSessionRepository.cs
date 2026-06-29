using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class LoginSessionRepository : ILoginSessionRepository
    {
        private readonly AppDbContext _context;

        public LoginSessionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            LoginSession session,
            CancellationToken cancellationToken)
        {
            await _context.LoginSessions.AddAsync(session, cancellationToken);
        }

        public async Task<LoginSession?> GetBySessionIdAsync(
            string sessionId,
            CancellationToken cancellationToken)
        {
            return await _context.LoginSessions
                .Include(x => x.User)
                .FirstOrDefaultAsync(
                    x => x.SessionId == sessionId,
                    cancellationToken);
        }

        public Task UpdateAsync(
            LoginSession session,
            CancellationToken cancellationToken)
        {
            _context.LoginSessions.Update(session);

            return Task.CompletedTask;
        }
    }
}
