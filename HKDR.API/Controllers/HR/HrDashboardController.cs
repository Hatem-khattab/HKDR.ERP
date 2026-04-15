using HKDR.API.Services.HrDashBoard;
using HKDR.Common.DTOs.DashBoard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/hr/dashboard")]
[Authorize]
public class HrDashboardController : ControllerBase
{
    private readonly IHrDashboardService _dashboardService;

    public HrDashboardController(IHrDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
    {
        var result = await _dashboardService.GetSummaryAsync();
        return Ok(result);
    }
}
