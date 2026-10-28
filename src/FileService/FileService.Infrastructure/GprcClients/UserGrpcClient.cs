using AuthService.Grpc;
using FileService.Application.DTOs;
using FileService.Application.Interfaces;

namespace FileService.Infrastructure.GprcClients
{
    public sealed class UserGrpcClient : IUserGrpcClient
    {
         private readonly UserGrpc.UserGrpcClient _client;
        public UserGrpcClient(UserGrpc.UserGrpcClient client)
        {
            _client = client;
        }
        public async Task<UserInfoDto> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var response = await _client.GetUserAsync(new GetUserRequest { UserId = userId.ToString() }, cancellationToken: cancellationToken);
            if (!response.Found)
                return null;

            return new UserInfoDto
            {
                UserId = Guid.Parse(response.UserId),
                FullName = response.FullName,
                IsActive = response.IsActive
            };
        }
    }
}
