using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class ProjectMember
    {
		public int userID { get; set; }
		public int projectID { get; set; }
		public virtual User User { get; set; }
		public virtual Project Project { get; set; }
	}
}
