namespace DcsExamPortal.Api.DTOs
{
    // DTO for returning dashboard data
    public class DashboardStatsDto
    {
        public int TotalUsers { get; set; }
        public decimal TotalPayments { get; set; }
        public int ActiveExams { get; set; }
        public List<RecentSubmissionDto> RecentSubmissions { get; set; } = new();
    }
}
