namespace HKDR.Common.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string[] Roles { get; set; } = Array.Empty<string>();
        public int CompanyId { get; set; } = 0;
    }
}
