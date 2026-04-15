using HKDR.Common.DTOs.DashBoard;

namespace HKDR.API.Services.HrDashBoard
{
    public interface IHrDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
    }
}
