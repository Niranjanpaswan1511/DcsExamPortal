using DcsExamPortal.Api.Models;
using DcsExamPortal.Api.DTOs;

namespace DcsExamPortal.Api.Services
{
    public interface ISubmissionService
    {
        ApiResponse CreateSubmission(Submission submission);
        ApiResponse GetAllSubmissions();
    }
}
