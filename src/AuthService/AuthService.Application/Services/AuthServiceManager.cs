using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services
{
    public class AuthServiceManager : IAuthServiceManager
    {
        public readonly UserManager<AppUser> _userManager;
        public AuthServiceManager(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<string> RegisterAsync(string email, string password)
        {
            var user = new AppUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return string.Join(",", result.Errors.Select(x => x.Description));

            return "Register success";
        }
        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return "User not found";

            var check = await _userManager.CheckPasswordAsync(user, password);

            if (!check)
                return "Wrong password";

            return "Login success";
        }
    }
}
