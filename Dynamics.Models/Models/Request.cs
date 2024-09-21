using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class Request
    {
		public int requestID { get; set; }
		public int userID { get; set; }
		public string content { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? creationDate { get; set; }
		public string location { get; set; }
		public string attachment { get; set; }
		public int isEmergency { get; set; }
		public int status { get; set; }
		public virtual User User { get; set; }
		public virtual Project Project { get; set; }
	}
}
