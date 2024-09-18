using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject
{
    public class OrganizationResource
    {
		public int resourceID { get; set; }
		public int organizationID { get; set; }
		public string resourceName { get; set; }
		public int? quantity { get; set; }
		public string unit { get; set; }
		public string donator { get; set; }
		public string contentTransaction { get; set; }
		public virtual Organization Organization { get; set; }
	}
}
