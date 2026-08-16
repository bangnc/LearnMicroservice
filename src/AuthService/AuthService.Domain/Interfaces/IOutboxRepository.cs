using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces
{
    public interface IOutboxRepository
    {
        Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default);
        Task<List<OutboxMessage>> GetPendingAsync(CancellationToken cancellationToken = default);
    }
}
