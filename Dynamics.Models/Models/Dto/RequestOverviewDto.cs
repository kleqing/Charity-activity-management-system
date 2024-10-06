namespace Dynamics.Models.Models.DTO;

public class RequestOverviewDto
{
    public Guid RequestId { get; set; }
    public string Title { get; set; }
    public string Username { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
}