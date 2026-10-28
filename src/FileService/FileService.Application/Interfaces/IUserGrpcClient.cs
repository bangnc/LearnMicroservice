using FileService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Application.Interfaces
{
     public interface IUserGrpcClient
    {
        Task<UserInfoDto> GetUserAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
