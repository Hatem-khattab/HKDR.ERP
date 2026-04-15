using Microsoft.AspNetCore.Mvc;

namespace HKDR.UI.Areas.Pages.SystemSelection.Controllers
{
    [Area("Pages")]
    public class SystemSelectionController : Controller
    {
        // GET: /Pages/SystemSelection/
        public IActionResult Index()
        {
            return View();
        }
    }
}
