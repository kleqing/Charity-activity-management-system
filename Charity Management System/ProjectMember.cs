using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charity_Management_System
{
    public class ProjectMember
    {
        public int projectID { get; set; }
        public int userID { get; set; }

        // Navigation Properties
        public virtual User user { get; set; }
        public virtual Project project { get; set; }
    }
}
