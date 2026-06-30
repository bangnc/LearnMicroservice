using AuthService.Application.Queries.Users.GetUsers;
using AuthService.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<AppUser, UserDto>();
             //.ForMember(d => d.UserName,
             //o => o.MapFrom(s=>s.Email));
        }
    }
}
