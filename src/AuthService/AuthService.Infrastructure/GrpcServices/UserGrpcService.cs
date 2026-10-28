using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Grpc;
using Grpc.Core;

namespace AuthService.Infrastructure.GrpcServices
{
    public class UserGrpcService: UserGrpc.UserGrpcBase
    {
        public override Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
        {
            var response = new GetUserResponse
            {
                UserId = request.UserId,
                FullName = "Test User",
                IsActive = true,
                Found = true
            };

            return Task.FromResult(response);
        }
    }
}
