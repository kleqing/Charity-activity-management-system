using Dynamics.Models.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models.ViewModel
{
    public class MyProjectVM
    {
        public List<ProjectOverviewDto> ProjectsILead { get; set; }
        public List<ProjectOverviewDto> ProjectsIAmMember { get; set; }
    }
}
