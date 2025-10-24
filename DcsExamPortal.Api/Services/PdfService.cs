using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using DcsExamPortal.Api.Models;
using System.Reflection.Metadata;
using Document = QuestPDF.Fluent.Document;
using DcsExamPortal.Api.DTOs;
using DcsExamPortal.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace DcsExamPortal.Api.Services
{
    public class PdfService : IPdfService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public PdfService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public byte[] GenerateReceipt(Payment payment)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Content().Padding(20).Column(col =>
                    {
                        col.Item().Text("Payment Receipt").FontSize(20).Bold();
                        col.Item().Text($"Payment ID: {payment.PaymentId}");
                        col.Item().Text($"Order ID: {payment.OrderId}");
                        col.Item().Text($"Amount: ₹{payment.Amount}");
                        col.Item().Text($"Status: {payment.Status}");
                        col.Item().Text($"Date: {payment.CreatedAt:dd-MM-yyyy}");
                    });
                });
            }).GeneratePdf();
        }

        public async Task<ApiResponse> GetDetailsByOrderId(string orderId)
        {
            var result = await (
                from u in _context.Users
                join s in _context.Submissions on u.Id equals s.UserId
                join f in _context.Forms on s.FormId equals f.Id
                join p in _context.Payments on s.Id equals p.SubmissionId
                where p.OrderId == orderId
                select new
                {
                    u.Name,
                    u.Email,
                    f.Title,
                    f.Description,
                    f.Fee,
                    p.PaymentId,
                    p.OrderId,
                    p.CreatedAt
                }
            ).FirstOrDefaultAsync();

            if (result == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "No record found for given Order ID",
                    Data = null
                };
            }

            return new ApiResponse
            {
                Success = true,
                Message = "Payment details fetched successfully",
                Data = result
            };
        }


    }
}
