using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HKDR.API.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected int CompanyId =>
            int.Parse(User.FindFirst("CompanyId")!.Value);
    }
}
