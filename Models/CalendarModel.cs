using System.ComponentModel.DataAnnotations;

namespace MyApplicatioon.Models
{
    public class CalendarModel
    {
        [Key, Required]
        public Guid Id { get; set; }
        public string Event { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}