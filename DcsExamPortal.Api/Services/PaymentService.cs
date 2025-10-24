using DcsExamPortal.Api.DTOs;
using DcsExamPortal.Api.Models;
using DcsExamPortal.Api.Data;
using Razorpay.Api;
using Payment = DcsExamPortal.Api.Models.Payment;

namespace DcsExamPortal.Api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public PaymentService(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        public ApiResponse CreateOrder(PaymentRequestDto dto)
        {
            RazorpayClient client = new RazorpayClient(_config["Razorpay:KeyId"], _config["Razorpay:KeySecret"]);
            var options = new Dictionary<string, object>
            {
                { "amount", dto.Amount * 100 },
                { "currency", "INR" },
                { "receipt", $"order_rcpt_{Guid.NewGuid()}" }
            };
            var order = client.Order.Create(options);
            return ApiResponse.Ok(order);
        }

        public ApiResponse SubmitPaymentDetails(PaymentDto dto)
        {
            var payment = new Payment
            {
                SubmissionId = dto.SubmissionId,
                Provider = dto.Provider,
                OrderId = dto.OrderId,
                PaymentId = dto.PaymentId,
                Amount = dto.Amount,
                Currency = dto.Currency,
                Status = dto.Status,
                Signature = dto.Signature,
                CreatedAt = DateTime.UtcNow
            };
            _context.Payments.Add(payment);
             _context.SaveChanges();
            return ApiResponse.Ok(dto, "PaymentDetailsUpdated");
        }

        public ApiResponse VerifyPayment(PaymentVerifyDto dto)
        {
            string keySecret = _config["Razorpay:KeySecret"];
            string body = dto.OrderId + "|" + dto.PaymentId;
            string generatedSignature = Utils.Utils.GenerateHmacSHA256(body, keySecret);

            if (generatedSignature == dto.Signature)
                return ApiResponse.Ok(null, "Payment verified successfully");
            else
                return ApiResponse.Error("Invalid payment signature");
        }
    }
}
