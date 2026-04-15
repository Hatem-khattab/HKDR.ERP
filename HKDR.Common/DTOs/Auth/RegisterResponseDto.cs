namespace HKDR.Common.DTOs.Auth
{
    public class RegisterResponseDto
    {
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int CompanyId { get; set; } = 0;
    }
}
