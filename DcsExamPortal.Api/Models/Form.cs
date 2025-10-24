using System.ComponentModel.DataAnnotations;

namespace DcsExamPortal.Api.Models
{
    public class Form
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Fields { get; set; }  // JSON string of field definitions
        [Required]
        public Decimal Fee { get; set; }
        public bool IsActive { get; set; } = true;

        public int CreatedBy { get; set; }
    }
}
