using DcsExamPortal.Api.DTOs;
using DcsExamPortal.Api.Models;

namespace DcsExamPortal.Api.Services
{
    public interface IPdfService
    {
        byte[] GenerateReceipt(Payment payment);
        Task<ApiResponse> GetDetailsByOrderId(string orderid);
    }
}
