using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Responses;

namespace BikeStoreWeb.Core.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
        Task<ServiceResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto);

    }
}
