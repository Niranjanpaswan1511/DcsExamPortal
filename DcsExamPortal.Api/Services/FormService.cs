using DcsExamPortal.Api.Data;
using DcsExamPortal.Api.Models;
using DcsExamPortal.Api.DTOs;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace DcsExamPortal.Api.Services
{
    public class FormService : IFormService
    {
        private readonly AppDbContext _context;
        public FormService(AppDbContext context) => _context = context;

        public ApiResponse GetAllForms() =>
            ApiResponse.Ok(_context.Forms.ToList());

        public ApiResponse GetFormById(int id)
        {
            var form = _context.Forms.Find(id);
            return form != null ? ApiResponse.Ok(form) : ApiResponse.Error("Form not found");
        }

        public ApiResponse CreateForm(Form form)
        {
            _context.Forms.Add(form);
            _context.SaveChanges();
            return ApiResponse.Ok(form, "Form created successfully");
        }

        ApiResponse IFormService.GetExamList()
        {
            var form = _context.Forms.ToList();
            return form != null ? ApiResponse.Ok(form) : ApiResponse.Error("Form not found");
        }
    }
}
