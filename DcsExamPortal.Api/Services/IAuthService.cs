using DcsExamPortal.Api.DTOs;

namespace DcsExamPortal.Api.Services
{
    public interface IAuthService
    {
        ApiResponse Register(RegisterDto dto);
        ApiResponse Login(LoginDto dto);
    }
}
