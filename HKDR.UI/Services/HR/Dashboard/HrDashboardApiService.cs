using HKDR.Common.DTOs.DashBoard;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace HKDR.UI.Services.HR.Dashboard
{
    public class HrDashboardApiService : IHrDashboardApiService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _context;

        public HrDashboardApiService(HttpClient http, IHttpContextAccessor context)
        {
            _http = http;
            _context = context;
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            // جلب الـ JWT من Session
            var token = _context.HttpContext?.Session.GetString("JWT");

            if (string.IsNullOrEmpty(token))
                throw new Exception("JWT not found in session");

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.GetAsync("api/hr/dashboard/summary");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"API ERROR: {response.StatusCode} - {content}");

            return JsonConvert.DeserializeObject<DashboardSummaryDto>(content)!;
        }
    }
}
