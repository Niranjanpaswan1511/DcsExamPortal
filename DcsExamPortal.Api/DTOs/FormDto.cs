using System.ComponentModel.DataAnnotations;

namespace DcsExamPortal.Api.DTOs
{
    public class FormDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public Decimal Fee { get; set; }
    }
}
