using HKDR.DomainEntities.Entities.Core;
using System.Security.Claims;

namespace HKDR.API.Services.Auth
{
    public interface ITokenService
    {
        string GenerateAccessToken(ApplicationUser user, IList<string> roles);
        RefreshToken GenerateRefreshToken(ApplicationUser user);
    }

}
