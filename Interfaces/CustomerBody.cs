using System.ComponentModel.DataAnnotations;

namespace MyApplicatioon.Interfaces
{
    public class CustomerBody
    {
        [Required(ErrorMessage = "email is required")]
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerStage { get; set; }
    }
}