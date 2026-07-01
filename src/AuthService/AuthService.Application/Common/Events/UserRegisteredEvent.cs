using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Common.Events
{
    public sealed class UserRegisteredEvent
    {
        public string UserId { get; init; } = "";

        public string Email { get; init; } = "";

        public string FullName { get; init; } = "";
    }
}
