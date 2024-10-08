using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models.ViewModel
{
    public class MyProjectVM
    {
        public List<Project> ProjectsILead { get; set; }
        public List<Project> ProjectsIAmMember { get; set; }
        public List<Project> OtherProjects { get; set; }
    }
}
