using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models.DTO
{
    
    public class UpdateProjectProfileRequestDto
    {
        [ValidateNever]
        public Guid ProjectID { get; set; }
        [ValidateNever]
        public string ProjectName { get; set; }
        [ValidateNever]
        public int ProjectStatus { get; set; }
        [ValidateNever]
        public string? Attachment { get; set; }
        [ValidateNever]
        public string? ProjectDescription { get; set; }
        [DataType(DataType.Date)]
        [ValidateNever]
        public DateOnly? StartTime { get; set; }

        [DataType(DataType.Date)]
        [ValidateNever]
        public DateOnly? EndTime { get; set; }
        [ValidateNever]
        public Guid LeaderID { get; set; }
        [ValidateNever]
        public Guid OldLeaderID { get; set; }
        [DataType(DataType.PhoneNumber)]
        [ValidateNever]
        public string? PhoneNumber { get; set; }
        [ValidateNever]
        public string? Address { get; set; }
        [DataType(DataType.EmailAddress)]
        [ValidateNever]
        public string? Email { get; set; }
        [ValidateNever]
        public string? ReportFile { get; set; }
    }
}
