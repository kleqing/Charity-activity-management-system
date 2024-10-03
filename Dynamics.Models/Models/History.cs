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
        public Guid HistoryID { get; set; }
        public Guid ProjectID { get; set; }
        public string Phase { get; set; }//tên phase
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }//ngày bắt đầu phase
        public string Content { get; set; }//description phase
        public string? Attachment { get; set; }//ảnh
        public virtual Project Project { get; set; }
    }
}
