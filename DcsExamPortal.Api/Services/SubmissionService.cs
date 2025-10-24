using DcsExamPortal.Api.Data;
using DcsExamPortal.Api.DTOs;
using DcsExamPortal.Api.Models;

namespace DcsExamPortal.Api.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly AppDbContext _context;
        public SubmissionService(AppDbContext context) => _context = context;

        public ApiResponse CreateSubmission(Submission submission)
        {
            _context.Submissions.Add(submission);
            _context.SaveChanges();
            return ApiResponse.Ok(submission, "Form submitted successfully");
        }

        public ApiResponse GetAllSubmissions() =>
            ApiResponse.Ok(_context.Submissions.ToList());
    }
}
