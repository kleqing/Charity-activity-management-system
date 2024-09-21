using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class Award
    {
        public string awardID { get; set; }
        public string awardName { get; set; }
        public int userID { get; set; }
        public virtual User User { get; set; }
    }
}
