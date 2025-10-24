using DcsExamPortal.Api.DTOs;
using DcsExamPortal.Api.Models;
using DcsExamPortal.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace DcsExamPortal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormController : ControllerBase
    {
        private readonly IFormService _formService;

        public FormController(IFormService formService)
        {
            _formService = formService;
        }


        [HttpGet]
        public IActionResult GetAllForms()
        {
            var result = _formService.GetAllForms();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetFormById(int id)
        {
            var result = _formService.GetFormById(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateForm([FromBody] Form form)
        {
            if (form == null)
                return BadRequest(ApiResponse.Error("Invalid form data."));

            var result = _formService.CreateForm(form);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("Formlist")]
        public IActionResult FormList()
        {
            var result = _formService.GetExamList();
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
