using DcsExamPortal.Api.DTOs;

namespace DcsExamPortal.Api.Services
{
    public interface IPaymentService
    {
        ApiResponse CreateOrder(PaymentRequestDto dto);
        ApiResponse VerifyPayment(PaymentVerifyDto dto);
        ApiResponse SubmitPaymentDetails(PaymentDto dto);
    }
}
