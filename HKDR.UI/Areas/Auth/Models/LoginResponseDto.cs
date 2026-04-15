public class LoginResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public List<string> Roles { get; set; }
    public int CompanyId { get; set; }
    public string Email { get; set; }          
    public string CompanyName { get; set; }    
}
