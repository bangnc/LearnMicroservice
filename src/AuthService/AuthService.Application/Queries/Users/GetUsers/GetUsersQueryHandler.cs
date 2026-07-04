using AuthService.Application.DTOs.Page;
using AuthService.Application.Extensions;
using AuthService.Domain.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Queries.Users.GetUsers
{
    public class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, PageResponse<UserDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AutoMapper.IConfigurationProvider _mapperConfig;

        public GetUsersQueryHandler(UserManager<AppUser> userManager, AutoMapper.IConfigurationProvider mapperConfig)
        {
            _userManager = userManager;
            _mapperConfig = mapperConfig;
        }

        public async Task<PageResponse<UserDto>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
        {
            var query = _userManager.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.UserName!.Contains(request.Search)
                    || x.Email!.Contains(request.Search)
                    || x.FullName!.Contains(request.Search));
            }

            query = query.OrderByProperty(
                request.SortBy ?? nameof(AppUser.UserName),
                request.Desc);

            return await query

                .ProjectTo<UserDto>(_mapperConfig)
                .ToPagedResultAsync(request, cancellationToken);
        }
    }
}
