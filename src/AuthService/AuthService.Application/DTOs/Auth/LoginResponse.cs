using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.DTOs.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public bool RequiresOtp { get; set; }
        public Guid SessionId { get; set; }
    }
}
