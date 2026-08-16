using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace AuthService.Infrastructure.Repositories
{
    public class OutboxRepository : IOutboxRepository
    {
        private readonly AppDbContext _context;

        public OutboxRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            OutboxMessage message,
            CancellationToken cancellationToken = default)
        {
            await _context.OutboxMessages.AddAsync(message, cancellationToken);
        }

        public async Task<List<OutboxMessage>> GetPendingAsync(
                CancellationToken cancellationToken = default)
        {
            return await _context.OutboxMessages
                .Where(x => x.ProcessedAt == null && x.FailedAt == null)
                .OrderBy(x => x.CreatedAt)
                .Take(100)
                .ToListAsync(cancellationToken);
        }
    }
}
