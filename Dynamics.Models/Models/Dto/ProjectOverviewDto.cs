namespace Dynamics.Models.Models.Dto;

public class ProjectOverviewDto
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; }
    public string ProjectUser { get; set; }
    public string ProjectLocation { get; set; }
    public string ProjetDescription { get; set; }
    public int ProjectMembers { get; set; }
    public int? ProjectProgress { get; set; }
    public int ProjectRaisedMoney { get; set; }
    public int ProjectStatus { get; set; }
    public string ProjectAttachment { get; set; }
}