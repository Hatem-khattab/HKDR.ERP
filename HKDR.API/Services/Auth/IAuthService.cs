using HKDR.Common.DTOs.Auth;

namespace HKDR.API.Services.Auth
{
    
       
        public interface IAuthService
        {
            Task<LoginResponseDto> LoginAsync(LoginDto dto);
            Task<RegisterResponseDto> RegisterAsync(RegisterDto dto, string currentCompanyId);
            Task<LoginResponseDto> RefreshTokenAsync(string refreshToken);
            Task<bool> LogoutAsync(string refreshToken);
        
    }
}

