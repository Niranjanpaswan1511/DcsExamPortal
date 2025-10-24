using DcsExamPortal.Api.DTOs;
using DcsExamPortal.Api.Models;
using DcsExamPortal.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DcsExamPortal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;

        public SubmissionController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }


        [HttpPost]
        public IActionResult SubmitForm([FromBody] Submission submission)
        {
            if (submission == null)
                return BadRequest(ApiResponse.Error("Invalid submission data."));

            var result = _submissionService.CreateSubmission(submission);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAllSubmissions()
        {
            var result = _submissionService.GetAllSubmissions();
            return Ok(result);
        }
    }
}
