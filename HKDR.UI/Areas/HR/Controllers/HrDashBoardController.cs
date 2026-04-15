using HKDR.UI.Services.HR.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HKDR.UI.Areas.HR.Controllers
{
    [Area("HR")]
    
    public class HrDashBoardController : Controller
    {
        private readonly IHrDashboardApiService _dashboard;
        public HrDashBoardController(IHrDashboardApiService dashboard)
        {
            _dashboard = dashboard;
        }
        public async Task<IActionResult> HrDashBoard()
        {
            var summary = await _dashboard.GetSummaryAsync();
            return View(summary);
        }
    }
}
