using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charity_Management_System
{
    public class Request
    {
        public int requestID { get; set; }
        public int userID { get; set; }
        public string content { get; set; }
        public DateTime creationDate { get; set; }
        public string location { get; set; }
        public string attachment { get; set; }
        public bool isEmergency { get; set; }
        public int status { get; set; }
        public virtual User user { get; set; }
        public virtual Project project { get; set; }
    }
}
