namespace Dynamics.Models.Models.Dto;

public class OrganizationOverviewDto
{
    public Guid OrganizationId { get; set; }
    public string OrganizationName { get; set; }
    public string OrganizationLeader { get; set; }
    public string OrganizationLocation { get; set; }
    public string OrganizationDesc { get; set; }
    public int OrganizationMembers { get; set; }
}