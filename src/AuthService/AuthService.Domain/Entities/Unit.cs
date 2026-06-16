using AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class Unit : BaseEntity
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
    }
}
