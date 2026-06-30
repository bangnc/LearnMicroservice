using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Queries.Users.GetUsers
{
    public class GetUsersQuery : IRequest<List<UserDto>>
    {
    }
}
