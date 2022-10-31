using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApplicatioon.Interfaces
{
    public class CompaignBody
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Messages { get; set; }
        public int TotalCustomers { get; set; }

    }
}