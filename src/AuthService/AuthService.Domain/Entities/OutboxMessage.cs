using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }

        public string EventType { get; set; } = default!;

        public string Payload { get; set; } = default!;

        public DateTime CreatedAt { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public int RetryCount { get; set; }

        public DateTime? FailedAt { get; set; }

        public string? Error { get; set; }
    }
}
