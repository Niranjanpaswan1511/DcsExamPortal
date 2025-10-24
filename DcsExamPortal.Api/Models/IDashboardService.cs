using DcsExamPortal.Api.DTOs;

namespace DcsExamPortal.Api.Models
{
    public interface IDashboardService
    {
        DashboardStatsDto GetAdminDashboardStats();
    }
}
