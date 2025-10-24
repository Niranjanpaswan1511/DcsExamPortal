namespace DcsExamPortal.Api.DTOs
{
    public class PaymentDto
    {
        public int SubmissionId { get; set; }
        public string Provider { get; set; } = "Razorpay";
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "INR";
        public string Status { get; set; } = "Paid";
        public string Signature { get; set; }
    }
}
