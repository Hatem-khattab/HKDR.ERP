using HKDR.Common.DTOs.Auth;
using HKDR.DomainEntities.Entities.Core;
using HKDR.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HKDR.API.Services.Auth
{
  

    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly ERPDbContext _context;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService,
            ERPDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _context = context;
        }

        // ================= LOGIN =================
        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _tokenService.GenerateAccessToken(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken(user);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                Roles = roles.ToArray(),
                CompanyId = user.CompanyId
            };
        }

        // ================= REGISTER =================
        public async Task<RegisterResponseDto> RegisterAsync(RegisterDto dto, string currentCompanyId)
        {
            if (!await _roleManager.RoleExistsAsync(dto.Role))
                throw new Exception("Role does not exist");

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                CompanyId = int.Parse(currentCompanyId),
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, dto.Role);

            return new RegisterResponseDto
            {
                Email = user.Email,
                Role = dto.Role,
                CompanyId = user.CompanyId
            };
        }

        // ================= REFRESH =================
        public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var existingToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (existingToken == null || existingToken.IsRevoked || existingToken.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid refresh token");

            var user = existingToken.User!;
            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _tokenService.GenerateAccessToken(user, roles);

            return new LoginResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = existingToken.Token,
                Roles = roles.ToArray(),
                CompanyId = user.CompanyId
            };
        }

        // ================= LOGOUT =================
        public async Task<bool> LogoutAsync(string refreshToken)
        {
            var existingToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (existingToken == null) return false;

            existingToken.IsRevoked = true;
            _context.RefreshTokens.Update(existingToken);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
