using Azure;
using DcsExamPortal.Api.Models;
using DcsExamPortal.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DcsExamPortal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IDashboardService _authService;
        private readonly ISubmissionService _submissionService;

        public AdminController(IDashboardService authService, ISubmissionService submissionService)
        {
            _authService = authService;
            _submissionService = submissionService;
        }
        //[Authorize("Admin")]
        [HttpGet("admin-stats")]
        public IActionResult GetAdminStats()
        {
            try
            {
                var result = _authService.GetAdminDashboardStats();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving dashboard stats", error = ex.Message });
            }
        }
        //[Authorize("Admin")]
        [HttpGet("user-sub-list")]
        public IActionResult UserSubmission()
        {
            var response = _submissionService.GetAllSubmissions();

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
