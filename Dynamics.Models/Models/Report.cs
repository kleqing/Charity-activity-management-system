﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class Report
    {
        public Guid ReportID { get; set; }
        public Guid? ReporterID { get; set; }  // User who is reporting
        public Guid ObjectID { get; set; }
        public string Type { get; set; }
        public string Reason { get; set; }
        public DateTime ReportDate { get; set; }
        public virtual User? Reporter { get; set; }

    }
}
