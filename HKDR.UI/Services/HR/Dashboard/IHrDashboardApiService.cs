using HKDR.Common.DTOs.DashBoard;

namespace HKDR.UI.Services.HR.Dashboard
{
    public interface IHrDashboardApiService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
    }

}
