using AuthService.Application.DTOs.Page;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Queries.Users.GetUsers
{
    public class GetUsersQuery
        : PageRequest,
          IRequest<PageResponse<UserDto>>
    {
    }
}
