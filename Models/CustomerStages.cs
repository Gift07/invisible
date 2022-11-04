using MyApplicatioon.Models;

namespace MyApplicatioon.Models
{
    public class CustomerStages
    {
        public Guid Id { get; set; }
        public string CustomerStage { get; set; }
        public List<CustomerModel> Customers { get; set; }
    }
}