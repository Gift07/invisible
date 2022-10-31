using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApplicatioon.Models
{
    public class MessageModel
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public CompainModel compain { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}