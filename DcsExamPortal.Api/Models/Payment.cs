using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DcsExamPortal.Api.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Submission")]
        public int SubmissionId { get; set; }

        public string Provider { get; set; } = "Razorpay";

        public string OrderId { get; set; }

        public string PaymentId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "INR";

        public string Status { get; set; }

        public string Signature { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
