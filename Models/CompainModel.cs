using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApplicatioon.Models
{
    public class CompainModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Messages { get; set; }
        public int TotalCustomers { get; set; }
        public bool HasEnded { get; set; }
        public List<Guid> TotalClicks { get; set; }
        public List<Guid> DeliveredEmail { get; set; }
        public double ClickThroughRate { get; set; }
        public double ClickToOpenRate { get; set; }
        public double ComplaintRate { get; set; }
        public double ConversionRate { get; set; }
        public double OpenRates { get; set; }
        public double UnsubscribeRate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}