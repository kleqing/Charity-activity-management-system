using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class ProjectMember
    {
		public string UserID { get; set; }
		public int ProjectID { get; set; }
		public virtual User User { get; set; }
		public virtual Project Project { get; set; }
	}
}
