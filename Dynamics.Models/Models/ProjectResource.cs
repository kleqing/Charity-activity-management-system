﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class ProjectResource
    {
		public Guid ResourceID { get; set; }
		public Guid ProjectID { get; set; }
		public string ResourceName { get; set; }
		public int? Quantity { get; set; }
		public int? ExpectedQuantity { get; set; }
		public string Unit { get; set; }
		public virtual Project Project { get; set; }
	}
}
