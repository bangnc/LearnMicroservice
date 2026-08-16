using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Common.Events
{
    public class UserRegisteredIntegrationEvent
    {
        public string UserId { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string FullName { get; set; } = default!;
    }
}
