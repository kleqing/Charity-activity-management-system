using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class OrganizationResource
    {
		public Guid ResourceID { get; set; }
		public Guid OrganizationID { get; set; }
		public string ResourceName { get; set; }
		public int? Quantity { get; set; }
		public string Unit { get; set; }
		public string ContentTransaction { get; set; }
		public int? ExpectedQuantity { get; set; }
        public virtual Organization Organization { get; set; }
	}
}
