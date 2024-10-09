using Dynamics.Models.Models;
using Dynamics.Models.Models.DTO;
using Dynamics.Models.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Dynamics.Services;

public interface IProjectService
{
    /**
     * Map to dto for display purposes (Card)
     */
    public ProjectOverviewDto MapToProjectOverviewDto(Project p);
    List<ProjectOverviewDto> MapToListProjectOverviewDto(List<Project> projects);
    //manage project overview-----------
    public Task<MyProjectVM> ReturnMyProjectVM(Guid userID);
    public Task<AllProjectsVM> ReturnAllProjectsVMs();
    public Task<bool>DeleteImage(string imgPath,Guid phaseID);
    public Task<string> UploadImages(List<IFormFile> images,string folder);
    public Task<DetailProjectVM> ReturnDetailProjectVM(Guid projectID);
    public Task<string> UpdateProjectProfileAsync(UpdateProjectProfileRequestDto updateProject, List<IFormFile> images);
    //manage member of project------------
    public List<User> FilterMemberOfProject(Expression<Func<ProjectMember, bool>> filter);
    public Task<string> SendJoinProjectRequest(Guid projectID, Guid memberID);
    public Task<bool>AcceptJoinProjectRequestAll(Guid projectID);
    public Task<bool>DenyJoinProjectRequestAll(Guid projectID);
    //manage transaction history of project--------
    public Task<List<UserToProjectTransactionHistory>> GetRandom5Donors(Guid projectID);
    public Task<SendDonateRequestVM> ReturnSendDonateRequestVM(Guid projectID, string donor);
    Task<bool> SendDonateRequest(SendDonateRequestVM sendDonateRequestVM);
    public Task<ProjectTransactionHistoryVM> ReturnProjectTransactionHistoryVM(Guid projectID);
    Task<bool>AcceptDonateProjectRequestAll(Guid projectID,string donor);
    Task<bool>DenyDonateProjectRequestAll(Guid projectID,string donor);
    //manage project resource--------------
    Task<string> UpdateProjectResourceType(ProjectResource projectResource);
    //manage project phase report---------
    //constraint project phase report
    Task<List<DateTime>> GetExistingReportDatesAsync(Guid projectID);
    Task<DateTime> GetReportDateAsync(Guid projectID, Guid historyID);
    //add and edit project phase report
    Task<string> AddProjectPhaseReportAsync(History history, List<IFormFile> images);
    Task<string> EditProjectPhaseReportAsync(History history, List<IFormFile> images);

}