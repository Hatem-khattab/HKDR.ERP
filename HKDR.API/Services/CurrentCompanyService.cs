using HKDR.Infrastructure.Abstractions;
using System.Security.Claims;

namespace HKDR.API.Services
{
   

    public class CurrentCompanyService : ICurrentCompanyService
    {
        public int CompanyId { get; }

        public CurrentCompanyService(IHttpContextAccessor accessor)
        {
            var companyClaim = accessor.HttpContext?
                .User?
                .FindFirst("CompanyId");

            if (companyClaim != null)
                CompanyId = int.Parse(companyClaim.Value);
        }
    }
}
