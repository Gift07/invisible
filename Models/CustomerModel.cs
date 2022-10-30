using System.ComponentModel.DataAnnotations;


namespace MyApplicatioon.Models
{
    public class CustomerModel
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "email is required")]
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerStage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}