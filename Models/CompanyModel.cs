using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using MyApplicatioon.Authentication;
using System.Linq;
using System.Threading.Tasks;

namespace MyApplicatioon.Models
{
    public class CompanyModel
    {
        [Key, Required]
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public ApplicationUser Manager { get; set; }
        public List<EmployeeModel> Employees { get; set; }
    }

    public class EmployeeModel
    {
        public Guid Id { get; set; }
        public ApplicationUser Employee { get; set; }
        public string Title { get; set; }

    }
}