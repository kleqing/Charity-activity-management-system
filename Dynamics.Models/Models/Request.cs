﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class Request
    {
		public Guid RequestID { get; set; }
		public Guid UserID { get; set; }
		public string Content { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string RequestTitle {  get; set; }
        [DataType(DataType.EmailAddress)]
        public string? RequestEmail { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? RequestPhoneNumber { get; set; }
        public string Location { get; set; }
		public string? Attachment { get; set; }
		public int? isEmergency { get; set; }
		public int Status { get; set; }
		public virtual User User { get; set; }
		public virtual Project Project { get; set; }
	}
}
