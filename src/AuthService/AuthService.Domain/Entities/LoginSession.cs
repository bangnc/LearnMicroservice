using AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class LoginSession : BaseEntity
    {
        public Guid Id { get; set; }

        public string SessionId { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; } = null!;

        public string Otp { get; set; } = null!;

        public DateTime ExpiredAt { get; set; }

        public int AttemptCount { get; set; }

        public bool IsVerified { get; set; }

        public virtual AppUser User { get; set; } = null!;
    }
}
