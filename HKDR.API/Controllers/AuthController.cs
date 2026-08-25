using HKDR.API.Services.Auth;
using HKDR.Common.DTOs.Auth;
using HKDR.DomainEntities.Entities.Core;
using HKDR.Infrastructure.Abstractions;
using HKDR.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace HKDR.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly ICurrentCompanyService _currentCompany;
        private readonly ERPDbContext _context;
        private readonly ITokenService _tokenService;
        public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config,
            ICurrentCompanyService currentCompany,
            ITokenService tokenService,
             ERPDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _currentCompany = currentCompany;
            _tokenService = tokenService;
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }

        // ================= REGISTER =================
        [Authorize(Roles = "SuperAdmin,CompanyAdmin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!await _roleManager.RoleExistsAsync(dto.Role))
                return BadRequest("Role does not exist");

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                CompanyId = _currentCompany.CompanyId,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, dto.Role);

            return Ok(new
            {
                Message = "User created successfully",
                user.Email,
                Role = dto.Role,
                CompanyId = user.CompanyId
            });
        }

        // ================= LOGIN =================
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {

            // 1️⃣ التحقق من وجود المستخدم
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized("Invalid credentials");

            // 2️⃣ جلب الأدوار
            var roles = await _userManager.GetRolesAsync(user);

            // 3️⃣ توليد Access Token
            var accessToken = _tokenService.GenerateAccessToken(user, roles);

            // 4️⃣ توليد Refresh Token وحفظه مباشرة في DB
            var refreshToken = _tokenService.GenerateRefreshToken(user);

            // 5️⃣ إرجاع النتيجة
            return Ok(new
            {
                accessToken = accessToken,
                refreshToken = refreshToken.Token,
                roles,
                companyId = user.CompanyId
            });

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.RefreshToken))
                return BadRequest("RefreshToken is required");

            // التأكد أن _context موجود
            if (_context == null)
                throw new Exception("_context is null");

            // جلب الـ RefreshToken من DB
            var existingToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == dto.RefreshToken);

            if (existingToken == null || existingToken.IsRevoked || existingToken.ExpiresAt < DateTime.UtcNow)
                return Unauthorized("Invalid refresh token");

            // جلب المستخدم
            var user = await _userManager.FindByIdAsync(existingToken.UserId);
            if (user == null)
                return Unauthorized("User not found");

            // توليد Access Token جديد
            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _tokenService.GenerateAccessToken(user, roles);

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = existingToken.Token,
                expiresAt = existingToken.ExpiresAt
            });
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RevokeTokenDto dto)
        {
            if (string.IsNullOrEmpty(dto.RefreshToken))
                return BadRequest("RefreshToken is required");

            // جلب RefreshToken من DB
            var existingToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == dto.RefreshToken);

            if (existingToken == null)
                return NotFound("Refresh token not found");

            // إلغاء صلاحية الـ Token
            existingToken.IsRevoked = true;
            _context.RefreshTokens.Update(existingToken);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Logout successful, refresh token revoked" });
        }

    }

}

