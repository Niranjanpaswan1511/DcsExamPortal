using DcsExamPortal.Api.DTOs;
using DcsExamPortal.Api.Models;
using Microsoft.Data.SqlClient;

namespace DcsExamPortal.Api.Services
{
    public class DashboardService:IDashboardService
    {
        private readonly IConfiguration _config;

        public DashboardService(IConfiguration config)
        {
            _config = config;
        }

        public DashboardStatsDto GetAdminDashboardStats()
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");
            var stats = new DashboardStatsDto();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // ✅ Total Registered Users
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users", con))
                {
                    stats.TotalUsers = (int)cmd.ExecuteScalar();
                }

                // ✅ Total Payments
                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(Amount), 0) FROM Payments", con))
                {
                    stats.TotalPayments = Convert.ToDecimal(cmd.ExecuteScalar());
                }

                // ✅ Active Exams
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Forms WHERE IsActive = 1", con))
                {
                    stats.ActiveExams = (int)cmd.ExecuteScalar();
                }

                // ✅ Recent Submitted Forms (Top 5)
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT TOP 5 s.Id, u.Name AS UserName, f.Title AS FormTitle, s.CreatedAt
                    FROM Submissions s
                    JOIN Users u ON s.UserId = u.Id
                    JOIN Forms f ON s.FormId = f.Id
                    ORDER BY s.CreatedAt DESC", con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stats.RecentSubmissions.Add(new RecentSubmissionDto
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserName = reader["UserName"].ToString(),
                                FormTitle = reader["FormTitle"].ToString(),
                                SubmittedAt = Convert.ToDateTime(reader["CreatedAt"]).ToString("g")
                            });
                        }
                    }
                }
            }

            return stats;
        }
    }

}
