using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class History
    {
        public string HistoryID { get; set; }
        public string ProjectID { get; set; }
        public string Phase { get; set; }
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }
        public string Content { get; set; }
        public string Attachment { get; set; }
        public virtual Project Project { get; set; }
    }
}
