using System.ComponentModel.DataAnnotations;

namespace Dynamics.Models.Models.DTO;

public class RequestOverviewDto
{
    public Guid RequestID { get; set; }
    public string Username { get; set; }
    public Guid UserID { get; set; }
    public string RequestTitle { get; set; }
    public string Content { get; set; }
    public string Location { get; set; }
    public string? FirstImageAttachment { get; set; }
    public int? isEmergency { get; set; }
}