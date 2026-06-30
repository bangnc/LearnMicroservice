using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface IJwtService
    {
        string CreateToken(AppUser user);
        string CreateRefreshToken(AppUser user);
        string CreateTempToken(AppUser user, string sessionId);
        ClaimsPrincipal ValidateToken(string token);
    }
}
