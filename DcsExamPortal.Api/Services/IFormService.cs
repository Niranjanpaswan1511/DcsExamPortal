using DcsExamPortal.Api.DTOs;
using DcsExamPortal.Api.Models;

namespace DcsExamPortal.Api.Services
{
    public interface IFormService
    {
        ApiResponse GetAllForms();
        ApiResponse GetFormById(int id);
        ApiResponse CreateForm(Form form);
        ApiResponse GetExamList();
    }
}
