using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DcsExamPortal.Api.Models
{
    public class Submission
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Form")]
        public int FormId { get; set; }

        public string Answers { get; set; } // JSON of user answers

        public string Status { get; set; } = "Submitted";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
