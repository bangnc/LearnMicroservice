using AuthService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Queries.Users.GetUsers
{
    public class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, List<UserDto>>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetUsersQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserDto>> Handle(
            GetUsersQuery request,
            CancellationToken cancellationToken)
        {
            return await _userManager.Users
                .AsNoTracking()
                .OrderBy(x => x.UserName)
                .Select(x => new UserDto
                {
                    Id = x.Id,
                    UserName = x.UserName!,
                    Email = x.Email!,
                    FullName = x.FullName,
                    IsActive = x.IsActive,                    
                })
                .ToListAsync(cancellationToken);
        }
    }
}
