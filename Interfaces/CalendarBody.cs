using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApplicatioon.Interfaces
{
    public class CalendarBody
    {
        public string Event { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}