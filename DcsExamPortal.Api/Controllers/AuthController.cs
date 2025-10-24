using DcsExamPortal.Api.DTOs;
using DcsExamPortal.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DcsExamPortal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }   

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            var response = _authService.Register(dto);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var response = _authService.Login(dto);
            if (!response.Success)
                return Unauthorized(response);

            return Ok(response);
        }
    }
}
