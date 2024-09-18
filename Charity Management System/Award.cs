using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charity_Management_System
{
    public class Award
    {
        public int awardID { get; set; }
        public string awardType { get; set; }
        public int userID { get; set; }
        public virtual User user { get; set; }
    }
}
