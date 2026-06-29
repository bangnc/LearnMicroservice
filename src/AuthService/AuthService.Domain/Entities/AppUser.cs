using Microsoft.AspNetCore.Identity;

namespace AuthService.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        // mở rộng sau này
        public string? FullName { get; set; }
        public bool IsActive { get; set; } = true;
        // 👉 FK tới Unit
        public Guid? UnitId { get; set; }

        public virtual ICollection<LoginSession> LoginSessions { get; set; }
        = new List<LoginSession>();
    }
}
