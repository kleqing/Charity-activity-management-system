using AutoMapper;
using Dynamics.DataAccess.Repository;
using Dynamics.Models.Models;
using Dynamics.Models.Models.DTO;
using Dynamics.Models.Models.ViewModel;
using Dynamics.Services;
using Dynamics.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Linq.Expressions;
using System.Net.WebSockets;

using Dynamics.Helps;
using Dynamics.Models.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;
using Util = Dynamics.Utility.Util;

namespace Dynamics.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectRepository projectRepository;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IRequestRepository requestRepository;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IMapper mapper;
        private readonly IProjectService projectService;
        IOrganizationVMService _organizationService;

        public ProjectController(IProjectRepository projectRepository,
            IOrganizationRepository organizationRepository,
            IRequestRepository requestRepository,
            IWebHostEnvironment hostEnvironment,
            IMapper mapper,
            IProjectService projectService,
            IOrganizationVMService organizationService
           )
        {
            this.projectRepository = projectRepository;
            this.organizationRepository = organizationRepository;
            this.requestRepository = requestRepository;
            this.hostEnvironment = hostEnvironment;
            this.mapper = mapper;
            this.projectService = projectService;
            this._organizationService = organizationService;
        }

        [Route("Project/Index/{userID:guid}")]
        public async Task<IActionResult> Index(Guid userID)
        {
            //get all project
            var allProjects = await projectRepository.GetAllProjectsAsync();
            //get project that user has joined
            var projectMemberList =
                projectRepository.FilterProjectMember(
                    x => x.UserID.Equals(userID) && x.Status >= 1 && x.Project.ProjectStatus >= 0);
            List<ProjectOverviewDto> projectsIAmMember = new List<ProjectOverviewDto>();
            List<ProjectOverviewDto> projectsILead = new List<ProjectOverviewDto>();
            List<ProjectOverviewDto> otherProjects = new List<ProjectOverviewDto>();
            HashSet<Guid> userProjectIds = new HashSet<Guid>();

            foreach (var projectMember in projectMemberList)
            {
                  var project = await projectRepository.GetProjectAsync(x => x.ProjectID.Equals(projectMember.ProjectID));
                if (project != null)
                {
                    var ProjectMemberOfUser = projectRepository.FilterProjectMember(p => p.ProjectID.Equals(projectMember.ProjectID) && p.UserID.Equals(userID));
                    var statusProjectMemberOfUser = ProjectMemberOfUser?.FirstOrDefault()?.Status;
                    var dto = projectService.MapToProjectOverviewDto(project);
                    userProjectIds.Add(project.ProjectID); 
                    if (statusProjectMemberOfUser>=2)
                    {
                        //get project that user join as a leader
                        projectsILead.Add(dto);
                    }
                    else
                    {
                        //get project that user join as a member
                        projectsIAmMember.Add(dto);
                    }
                }
            }
            foreach (var project in allProjects)
            {
                if (!userProjectIds.Contains(project.ProjectID))
                {
                    var dto = projectService.MapToProjectOverviewDto(project);
                    otherProjects.Add(dto); // Add to otherProjects only if not in the user's projects
                }
            }

            return View(new MyProjectVM()
            {
                ProjectsILead = projectsILead, ProjectsIAmMember = projectsIAmMember, OtherProjects = otherProjects
            });
        }

        public async Task<IActionResult> ViewAllProjects()
        {
            var allProjects = await projectRepository.GetAllProjectsAsync();
            var allActiveProjectsDto = new List<ProjectOverviewDto>();
            foreach (var p in allProjects)
            {
                var dto = projectService.MapToProjectOverviewDto(p);
                if (dto.ProjectStatus >=0)
                {
                    allActiveProjectsDto.Add(dto);
                }
            }

            // TODO: For organization, wait for Tuan
            var result = new AllProjectsVM
            {
                allActiveProjects = allActiveProjectsDto
            };
            return View(result);
        }
        //update project profile
        public async Task<IActionResult> DeleteImage(string imgPath, Guid phaseID)
        {
            var currentProjectID = HttpContext.Session.GetString("currentProjectID");
            if (phaseID != Guid.Empty)
            {
                var allImagesOfPhase = await projectRepository.GetAllImagesAsync(phaseID, "Phase");
                var historyObj =
                    await projectRepository.GetAllPhaseReportsAsync(x => x.HistoryID.Equals(phaseID));
                if (historyObj != null && allImagesOfPhase != null)
                {
                    if (allImagesOfPhase.Length > 0)
                    {
                        foreach (var img in allImagesOfPhase.Split(','))
                        {
                            if (img.Equals(imgPath))
                            {
                                allImagesOfPhase = allImagesOfPhase.Replace(img + ",", "");
                            }
                        }

                        historyObj[0].Attachment = allImagesOfPhase;
                    }

                    var res = await projectRepository.EditPhaseReportAsync(historyObj[0]);
                    if (!res)
                    {
                        TempData[MyConstants.Error] = $"Fail to delete image {imgPath}!";
                        return RedirectToAction(nameof(EditProjectPhaseReport),
                            new { historyID = phaseID, projectID = currentProjectID });
                    }

                    TempData[MyConstants.Success] = $"Delete image {imgPath} successful!";
                    return RedirectToAction(nameof(EditProjectPhaseReport),
                        new { historyID = phaseID, projectID = currentProjectID });
                }
            }
            else
            {
                var allImagesOfProject =
                    await projectRepository.GetAllImagesAsync(new Guid(currentProjectID), "Project");
                var projectObj =
                    await projectRepository.GetProjectAsync(x => x.ProjectID.Equals(new Guid(currentProjectID)));
                if (projectObj != null && allImagesOfProject != null)
                {
                    if (allImagesOfProject.Length > 0)
                    {
                        foreach (var img in allImagesOfProject.Split(','))
                        {
                            if (img.Equals(imgPath))
                            {
                                allImagesOfProject = allImagesOfProject.Replace(img + ",", "");
                            }
                        }

                        projectObj.Attachment = allImagesOfProject;
                    }

                    var currentProjectLeaderID = new Guid(HttpContext.Session.GetString("currentProjectLeaderID"));
                    var res = await projectRepository.UpdateAsync(projectObj, currentProjectLeaderID);
                    if (!res)
                    {
                        TempData[MyConstants.Error] = $"Fail to delete image {imgPath}!";
                        return RedirectToAction(nameof(UpdateProjectProfile), new { id = currentProjectID });
                    }

                    TempData[MyConstants.Success] = $"Delete image {imgPath} successful!";
                    return RedirectToAction(nameof(UpdateProjectProfile), new { id = currentProjectID });
                }
            }

            TempData[MyConstants.Error] = $"Fail to delete image {imgPath}!";
            return RedirectToAction(nameof(ManageProject), new { id = currentProjectID });
        }

        [HttpPost]
        public async Task<IActionResult> ImportFileProject(FinishProjectVM finishProjectVM, IFormFile reportFile)
        {
            var projectID = finishProjectVM.ProjectID;
            var projectObj =
                await projectRepository.GetProjectAsync(p => p.ProjectID.Equals(projectID));
            if (projectObj != null)
            {
                if (reportFile != null)
                {
                    var resReportFile = await Util.UploadFiles(new List<IFormFile> { reportFile }, @"files\Project");
                    if (resReportFile.Equals("No file"))
                    {
                        TempData[MyConstants.Error] = "No file to upload!";
                        return RedirectToAction(nameof(ManageProject), new { id = projectID });
                    }

                    if (resReportFile.Equals("Wrong extension"))
                    {
                        TempData[MyConstants.Error] = "Extension of some files is wrong!";
                        return RedirectToAction(nameof(ManageProject), new { id = projectID });
                    }

                    finishProjectVM.ReportFile = resReportFile;
                    var res = await projectRepository.FinishProjectAsync(finishProjectVM);
                    if (res)
                    {
                        TempData[MyConstants.Success] = "Finish project successfully!";
                        return RedirectToAction(nameof(ManageProject), new { id = projectID });
                    }
                }
            }

            TempData[MyConstants.Error] = "Fail to finish project!";
            return RedirectToAction(nameof(ManageProject), new { id = projectID });
        }

        public async Task<IActionResult> DownloadFile(string fileWebPath)
        {
            var currentProjectID = HttpContext.Session.GetString("currentProjectID");
            if (!string.IsNullOrEmpty(fileWebPath))
            {
                var fileName = fileWebPath.Substring(fileWebPath.LastIndexOf('/') + 1);
                var filepath = Path.Combine(hostEnvironment.WebRootPath, "files\\Project", fileName);
                var fileExtension = Path.GetExtension(filepath);
                if (!string.IsNullOrEmpty(fileExtension))
                {
                    switch (fileExtension)
                    {
                        case ".xlsx":
                            return File(System.IO.File.ReadAllBytes(filepath),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                System.IO.Path.GetFileName(filepath));
                        case ".xls":
                            return File(System.IO.File.ReadAllBytes(filepath), "application/vnd.ms-excel",
                                System.IO.Path.GetFileName(filepath));
                        case ".pdf":
                            return File(System.IO.File.ReadAllBytes(filepath), "application/pdf",
                                System.IO.Path.GetFileName(filepath));
                        case ".docx":
                            return File(System.IO.File.ReadAllBytes(filepath),
                                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                                System.IO.Path.GetFileName(filepath));
                        case ".doc":
                            return File(System.IO.File.ReadAllBytes(filepath), "application/msword",
                                System.IO.Path.GetFileName(filepath));
                        case ".txt":
                            return File(System.IO.File.ReadAllBytes(filepath), "text/plain",
                                System.IO.Path.GetFileName(filepath));
                    }

                    TempData[MyConstants.Success] = "Download file successful!";
                    return RedirectToAction(nameof(UpdateProjectProfile), new { id = new Guid(currentProjectID) });
                }
            }

            TempData[MyConstants.Info] = "There is no file to download!";
            return RedirectToAction(nameof(UpdateProjectProfile), new { id = new Guid(currentProjectID) });
        }

        public async Task<IActionResult> UpdateProjectProfile(Guid id)
        {
            var projectObj =
                await projectRepository.GetProjectAsync(p => p.ProjectID.Equals(id));
            if (projectObj == null)
            {
                return NotFound();
            }

            var projectDto = mapper.Map<UpdateProjectProfileRequestDto>(projectObj);
            IEnumerable<SelectListItem> StatusList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Pending", Value = "0" },
                new SelectListItem { Text = "In Progress", Value = "1" },
                new SelectListItem { Text = "Completed", Value = "2" }
            };
            ViewData["StatusList"] = StatusList;

            List<User> memberList =
                projectRepository.FilterMemberOfProject(x => x.ProjectID.Equals(id) && x.Status >= 1);
            ICollection<SelectListItem> MemberList = new List<SelectListItem>() { };
            foreach (var member in memberList)
            {
                MemberList.Add(new SelectListItem { Text = member.UserFullName, Value = member.UserID.ToString() });
            }

            ViewData["MemberList"] = MemberList;

            //get leader id from project member to updateProjectDto
            projectDto.NewLeaderID = new Guid(HttpContext.Session.GetString("currentProjectLeaderID"));
            return View(projectDto);
        }

        //POST: Project/UpdateProjectProfile
        [HttpPost]
        public async Task<IActionResult> UpdateProjectProfile(UpdateProjectProfileRequestDto updateProject,
            List<IFormFile> images)
        {
            var existingObj = await projectRepository.GetProjectAsync(p => p.ProjectID.Equals(updateProject.ProjectID));
            var projectObj = mapper.Map<Dynamics.Models.Models.Project>(updateProject);
            if (projectObj != null && existingObj != null)
            {
                if (images != null && images.Count() > 0)
                {
                    var resAttachment = await Util.UploadImages(images, @"images\Project");
                    if (resAttachment.Equals("No file"))
                    {
                        TempData[MyConstants.Error] = "No file to upload!";
                        return RedirectToAction(nameof(UpdateProjectProfile), new { id = projectObj.ProjectID });
                    }

                    if (resAttachment.Equals("Wrong extension"))
                    {
                        TempData[MyConstants.Error] = "Extension of some files is wrong!";
                        return RedirectToAction(nameof(UpdateProjectProfile), new { id = projectObj.ProjectID });
                    }

                    projectObj.Attachment = resAttachment;
                }

                var res = await projectRepository.UpdateAsync(projectObj, updateProject.NewLeaderID);
                if (!updateProject.OldLeaderID.Equals(updateProject.NewLeaderID) && res)
                {
                    TempData[MyConstants.Success] = "Update project successfully!";
                    //TempData[MyConstants.Info] = "You are no longer a leader for this project!";
                    return RedirectToAction(nameof(ManageProject), new { id = projectObj.ProjectID.ToString() });
                }
                TempData[MyConstants.Success] = "Update project successfully!";
                return RedirectToAction(nameof(ManageProject), new { id = projectObj.ProjectID.ToString() });
            }

            TempData[MyConstants.Error] = "Fail to update project!";
            return RedirectToAction(nameof(UpdateProjectProfile), new { id = projectObj.ProjectID });
        }

        [HttpPost]
        public async Task<IActionResult> ShutdownProject(ShutdownProjectVM shutdownProjectVM)
        {
            var userIDString = HttpContext.Session.GetString("currentUserID");
            var res = await projectRepository.ShutdownProjectAsync(shutdownProjectVM);
            if (res&&!string.IsNullOrEmpty(userIDString))
            {
                return Json(new { success = true, message = "Shutdown project successful!",
                    remind = "You just have shut down a project for \"" + shutdownProjectVM.Reason+"\"",
                    userIDString = userIDString
                });
            }
            return Json(new { success = false, message = "Fail to shutdown project!" });
        }

        public async Task<IActionResult> SendReportProjectRequest(Report report)
        {
            report.ReporterID = new Guid(HttpContext.Session.GetString("currentUserID"));
            report.Type = ReportObjectConstant.Project;
            var res = await projectRepository.SendReportProjectRequestAsync(report);
            if (res)
            {
                TempData[MyConstants.Success] = "Send report project request successfully!";
                return RedirectToAction(nameof(ManageProject), new { id = report.ObjectID });
            }

            TempData[MyConstants.Error] = "Fail to send report project request!";
            return RedirectToAction(nameof(ManageProject), new { id = report.ObjectID });
        }

        //show tổng quan project
        public async Task<IActionResult> ManageProject(string id)
        {
            if (string.IsNullOrEmpty(id.ToString()) || id.Equals(00000000 - 0000 - 0000 - 0000 - 000000000000))
            {
                return NotFound("id is empty!");
            }

            var projectObj =
                await projectRepository.GetProjectAsync(p => p.ProjectID.Equals(new Guid(id)));
            if (projectObj != null)
            {
                // TODO
                var request = await requestRepository.GetAsync(r => r.RequestID.Equals(projectObj.RequestID));
                if (request != null)
                {
                    projectObj.Request = request;
                }                 
                HttpContext.Session.SetString("currentProjectID", projectObj.ProjectID.ToString());
                var leaderOfProject = await projectRepository.GetProjectLeaderAsync(projectObj.ProjectID);
                HttpContext.Session.SetString("currentProjectLeaderID", leaderOfProject.UserID.ToString());
                var ceoOfProject = projectRepository.FilterMemberOfProject(x => x.Status == 2 && x.ProjectID==projectObj.ProjectID);
                HttpContext.Session.SetString("currentProjectCEOID", ceoOfProject[0].UserID.ToString());

                List<string> statistic = await projectRepository.GetStatisticOfProject(projectObj.ProjectID);
                DetailProjectVM detailProjectVM = new DetailProjectVM()
                {
                    CurrentProject = projectObj,
                    CurrentLeaderProject = leaderOfProject,
                    CurrentAmountOfMoneyDonate = Convert.ToInt32(statistic[0]),
                    ExpectedAmountOfMoneyDonate = Convert.ToInt32(statistic[1]),
                    ProgressDonate = Convert.ToDouble(statistic[2]),
                    NumberOfProjectContributor = Convert.ToInt32(statistic[3]),
                    TimeLeftEndDay = Convert.ToInt32(statistic[3]),
                    Random5Donors = await projectRepository.GetRandom5Donors(projectObj.ProjectID)
                };
                if (detailProjectVM != null)
                {
                    return View(detailProjectVM);
                }
            }

            TempData[MyConstants.Error] = "Fail to get project!";
            return RedirectToAction(nameof(Index), new { id = HttpContext.Session.GetString("currentUserID") });
        }

        //----------------------manage project member -------------
        [Route("Project/ManageProjectMember/{projectID}")]
        public async Task<IActionResult> ManageProjectMember([FromRoute] Guid projectID)
        {
            var allProjectMember =
                projectRepository.FilterProjectMember(p => p.ProjectID.Equals(projectID) && p.Status >= 1);

            if (allProjectMember == null)
            {
                return NotFound();
            }

            var joinRequests =
                projectRepository.FilterProjectMember(p => p.ProjectID.Equals(projectID) && p.Status == 0) ??
                Enumerable.Empty<ProjectMember>();
            var nums = joinRequests.Count();
            ViewData["hasJoinRequest"] = nums > 0;
            return View(allProjectMember);
        }

        [Route("Project/DeleteProjectMember/{memberID}")]
        public async Task<IActionResult> DeleteProjectMember([FromRoute] Guid memberID)
        {
            var currentProjectID = HttpContext.Session.GetString("currentProjectID");
            var res = await projectRepository.DeleteMemberProjectByIdAsync(memberID, new Guid(currentProjectID));
            if (res)
            {
                TempData[MyConstants.Success] = "Delete project member successfully!";
                return RedirectToAction(nameof(ManageProjectMember), new { id = currentProjectID });
            }
            TempData[MyConstants.Error] = "Fail to delete project member!";
            return RedirectToAction(nameof(ManageProjectMember), new { id = currentProjectID });
        }

        //----manage join request-----
        //create request
        public async Task<IActionResult> JoinProjectRequest(Guid memberID, Guid projectID)
        {
            var projectObj =
                await projectRepository.GetProjectAsync(p => p.ProjectID.Equals(projectID));
            if (projectObj?.ProjectStatus == -1)
            {
                TempData[MyConstants.Warning] = "This project is not in progress!";
                return RedirectToAction(nameof(ManageProject), new { id = projectID });
            }

            var existingJoinRequest = projectRepository
                .FilterProjectMember(p => p.ProjectID.Equals(projectID) && p.UserID.Equals(memberID) && p.Status == 0
                 ).FirstOrDefault();

            if (existingJoinRequest == null)
            {
                var res = await projectRepository.AddJoinRequest(memberID, projectID);
                if (res)
                {
                    TempData[MyConstants.Success] = "Join request sent successfully!";
                    return RedirectToAction(nameof(ManageProject), new { id = projectID });
                }

                TempData[MyConstants.Error] = "Fail to send join request!";
                return RedirectToAction(nameof(ManageProject), new { id = projectID });
            }
            else
            {
                TempData[MyConstants.Warning] = "Already send join request.Please wait for response!";
                return RedirectToAction(nameof(ManageProject), new { id = projectID });
            }
        }

        //send notification to move out from project
        public async Task<IActionResult> LeaveProjectRequest(Guid projectID)
        {
            var currentUserID = HttpContext.Session.GetString("currentUserID");
            if (currentUserID != null)
            {
                var currentProjectLeaderID = HttpContext.Session.GetString("currentProjectLeaderID");
                if (currentUserID.Equals(currentProjectLeaderID))
                {
                    TempData[MyConstants.Warning] = "You can not leave the project while you are a project leader !";
                    TempData[MyConstants.Info] = "Transfer team leader rights if you still want to leave the project.";
                    return RedirectToAction(nameof(ManageProject), new { id = projectID });
                }

                var res = await projectRepository.DenyJoinRequestAsync(new Guid(currentUserID), projectID);
                if (res)
                {
                    TempData[MyConstants.Success] = "Move out project successfull!";
                    return RedirectToAction(nameof(ManageProject), new { id = projectID });
                }
            }

            TempData[MyConstants.Error] = "Fail to move out project!";
            return RedirectToAction(nameof(ManageProject), new { id = projectID });
        }

        //review request
        [Route("Project/ReviewJoinRequest/{projectID}")]
        public async Task<IActionResult> ReviewJoinRequest([FromRoute] Guid projectID)
        {
            var allJoinRequest =
                projectRepository.FilterProjectMember(p => p.ProjectID.Equals(projectID) && p.Status == 0);

            if (allJoinRequest == null)
            {
                return NotFound();
            }

            return View(allJoinRequest);
        }

        [Route("Project/AcceptedJoinRequest/{memberID}")]
        public async Task<IActionResult> AcceptedJoinRequest([FromRoute] Guid memberID)
        {
            var currentProjectID = new Guid(HttpContext.Session.GetString("currentProjectID"));
            var res = await projectRepository.AcceptedJoinRequestAsync(memberID, currentProjectID);
            if (res)
            {
                TempData[MyConstants.Success] = "Join request accepted successfully!";
                return RedirectToAction(nameof(ReviewJoinRequest), new { id = currentProjectID });
            }

            TempData[MyConstants.Error] = "Failed to accept the join request!";
            return RedirectToAction(nameof(ReviewJoinRequest), new { id = currentProjectID });
        }

        [Route("Project/DenyJoinRequest/{memberID}")]
        public async Task<IActionResult> DenyJoinRequest([FromRoute] Guid memberID)
        {
            var currentProjectID = new Guid(HttpContext.Session.GetString("currentProjectID"));
            var res = await projectRepository.DenyJoinRequestAsync(memberID, currentProjectID);
            if (res)
            {
                TempData[MyConstants.Success] = "Join request denied successfully!";
                return RedirectToAction(nameof(ReviewJoinRequest), new { id = currentProjectID });
            }

            TempData[MyConstants.Error] = "Failed to deny the join request!";
            return RedirectToAction(nameof(ReviewJoinRequest), new { id = currentProjectID });
        }

        public async Task<IActionResult> AcceptedJoinRequestAll()
        {
            var currentProjectID = HttpContext.Session.GetString("currentProjectID");
            var allJoinRequest =
                projectRepository.FilterProjectMember(
                    p => p.ProjectID.Equals(new Guid(currentProjectID)) && p.Status == 0);
            if (allJoinRequest == null)
            {
                return NotFound();
            }

            foreach (var joinRequest in allJoinRequest)
            {
                var res = await projectRepository.AcceptedJoinRequestAsync(joinRequest.UserID,
                    new Guid(currentProjectID));
                if (!res)
                {
                    TempData[MyConstants.Error] = "Failed to accept the join request!";
                    return RedirectToAction(nameof(ReviewJoinRequest), new { id = new Guid(currentProjectID) });
                }
            }

            TempData[MyConstants.Success] = "All join request accepted successfully!";
            return RedirectToAction(nameof(ReviewJoinRequest), new { id = new Guid(currentProjectID) });
        }

        public async Task<IActionResult> DenyJoinRequestAll()
        {
            var currentProjectID = HttpContext.Session.GetString("currentProjectID");
            var allJoinRequest =
                projectRepository.FilterProjectMember(
                    p => p.ProjectID.Equals(new Guid(currentProjectID)) && p.Status == 0);
            if (allJoinRequest == null)
            {
                return NotFound();
            }

            foreach (var joinRequest in allJoinRequest)
            {
                var res = await projectRepository.DenyJoinRequestAsync(joinRequest.UserID, new Guid(currentProjectID));
                if (!res)
                {
                    TempData[MyConstants.Error] = "Failed to deny the join request!";
                    return RedirectToAction(nameof(ReviewJoinRequest), new { id = new Guid(currentProjectID) });
                }
            }

            TempData[MyConstants.Success] = "All join request denied successfully!";
            return RedirectToAction(nameof(ReviewJoinRequest), new { id = new Guid(currentProjectID) });
        }

        //-------------------manage transaction history of project------------------------
        [HttpGet]
        public async Task<IActionResult> SendDonateRequest(Guid projectID, string donor)
        {
            var projectObj =
                await projectRepository.GetProjectAsync(p => p.ProjectID.Equals(projectID));
            if (projectObj?.ProjectStatus == -1)
            {
                TempData[MyConstants.Warning] = "This project is not in progress!";
                return RedirectToAction(nameof(ManageProject), new { id = projectID });
            }

            if (!ModelState.IsValid)
            {
                TempData[MyConstants.Error] = "Fail to send donate request!";
                return RedirectToAction(nameof(SendDonateRequest), new { projectID = projectID, donor = donor });
            }

            var allResource = await projectRepository.FilterProjectResourceAsync(p =>
                p.ProjectID.Equals(projectID) && !p.ResourceName.Equals("Money"));
            if (allResource == null)
            {
                return NotFound();
            }

            IEnumerable<SelectListItem> ResourceTypeList = allResource.Select(x => new SelectListItem
            {
                Text = x.ResourceName + " - " + x.Unit,
                Value = x.ResourceID.ToString(),
            }).ToList();
            ViewData["ResourceTypeList"] = ResourceTypeList;
            //get organization user lead
            var userString = HttpContext.Session.GetString("user");
            User currentUser = null;
            if (userString != null)
            {
                currentUser = JsonConvert.DeserializeObject<User>(userString);
            }
            var organizationUserLead = await organizationRepository.GetOrganizationUserLead(currentUser.UserID);

            //set value for View Model
            SendDonateRequestVM sendDonateRequestVM = null;
            if (!string.IsNullOrEmpty(donor) && donor.Equals("User"))
            {
                var currentUserID = HttpContext.Session.GetString("currentUserID");
                List<UserToProjectTransactionHistory> donateHistoryOfUser;
                if (currentUserID != null)
                {
                    donateHistoryOfUser = await projectRepository.GetAllUserDonateAsync(
                        u => u.UserID.Equals(new Guid(currentUserID)) && u.ProjectResource.ProjectID.Equals(projectID)
                   );
                    sendDonateRequestVM = new SendDonateRequestVM()
                    {
                        ProjectID = projectID,
                        TypeDonor = donor,
                        UserTransactionHistory = donateHistoryOfUser
                    };
                }
            } //if ceo click on the organization donate button
            else if (!string.IsNullOrEmpty(donor) && donor.Equals("Organization"))
            {
                var currentUserID = HttpContext.Session.GetString("currentUserID");
                List<OrganizationToProjectHistory> donateHistoryOfOrg;
                if (currentUserID != null&& organizationUserLead!=null)
                {

                     donateHistoryOfOrg = await projectRepository.GetAllOrganizationDonateAsync(
                        u => u.OrganizationResource.OrganizationID.Equals(organizationUserLead.OrganizationID) && u.ProjectResource.ProjectID.Equals(projectID)
                     );
                    sendDonateRequestVM = new SendDonateRequestVM()
                    {
                        ProjectID = projectID,
                        TypeDonor = donor,
                        OrgTransactionHistory = donateHistoryOfOrg,
                        OrganizationUserLeadID = organizationUserLead.OrganizationID
                    };                             
                }
            }

            return View(sendDonateRequestVM);
        }

        [HttpPost]
        public async Task<IActionResult> SendDonateRequest(SendDonateRequestVM sendDonateRequestVM)
        {
            if (sendDonateRequestVM != null)
            {
                if (!string.IsNullOrEmpty(sendDonateRequestVM.TypeDonor) &&
                    sendDonateRequestVM.TypeDonor.Equals("User"))
                {
                    var currentUserID = HttpContext.Session.GetString("currentUserID");
                    if (currentUserID != null)
                    {
                        var res = await projectRepository.AddUserDonateRequestAsync(sendDonateRequestVM.UserDonate);
                        if (!res)
                        {
                            // Return JSON response with failure message
                            return Json(new { success = false, message = "Fail to send your donation request!" });
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(sendDonateRequestVM.TypeDonor) &&
                         sendDonateRequestVM.TypeDonor.Equals("Organization"))
                {
                    var organizationResourceIDCoressponding = await organizationRepository.GetOrgResourceIDCorresponding(
                        sendDonateRequestVM.OrgDonate.ProjectResourceID.Value,
                        sendDonateRequestVM.OrganizationUserLeadID
                    );

                    if (!organizationResourceIDCoressponding.Equals(Guid.Empty))
                    {
                        sendDonateRequestVM.OrgDonate.OrganizationResourceID = organizationResourceIDCoressponding;
                        var res = await projectRepository.AddOrgDonateRequestAsync(sendDonateRequestVM.OrgDonate);
                        if (!res)
                        {
                            // Return JSON response with failure message
                            return Json(new { success = false, message = "Failed to send your donation request !" });
                        }
                    }
                    else
                    {
                        // Return JSON response with failure message for organization donation
                        return Json(new { success = false, message = "Failed to send your donation request !" });
                    }
                }

                // Return success response
                return Json(new { success = true, message = "Your donation request was sent successfully!" });
            }

            // Return failure response if the input data is invalid
            return Json(new { success = false, message = "Failed to send your donation request!" });
        }

        [Route("Project/ManageProjectDonor/{projectID}")]
        public async Task<IActionResult> ManageProjectDonor(Guid projectID)
        {
            var allUserDonate =
                await projectRepository.GetAllUserDonateAsync(u => u.ProjectResource.ProjectID.Equals(projectID) && u.Status == 1);
            if (allUserDonate == null)
            {
                return NotFound();
            }

            var allOrganizationDonate =
                await projectRepository.GetAllOrganizationDonateAsync(  u => u.ProjectResource.ProjectID.Equals(projectID) && u.Status == 1);
            if (allOrganizationDonate == null)
            {
                return NotFound();
            }

            ProjectTransactionHistoryVM projectTransactionHistoryVM = new ProjectTransactionHistoryVM()
            {
                UserDonate = allUserDonate,
                OrganizationDonate = allOrganizationDonate
            };
            int nums =
                (await projectRepository.GetAllUserDonateAsync(u => u.ProjectResource.ProjectID.Equals(projectID) && u.Status == 0) ?? new List<UserToProjectTransactionHistory>()).Count();
            var hasUserDonateRequest = nums > 0;

            int nums2 =
                (await projectRepository.GetAllOrganizationDonateAsync(
                     u => u.ProjectResource.ProjectID.Equals(projectID) && u.Status == 0) ??
                 new List<OrganizationToProjectHistory>()).Count();
            var hasOrgDonateRequest = nums2 > 0;

            ViewData["hasUserDonateRequest"] = hasUserDonateRequest;
            ViewData["hasOrgDonateRequest"] = hasOrgDonateRequest;
            return View(projectTransactionHistoryVM);
        }

        [HttpGet]
        [Route("Project/ReviewUserDonateRequest/{projectID}")]
        public async Task<IActionResult> ReviewUserDonateRequest(Guid projectID)
        {
            var allUserDonate =
                await projectRepository.GetAllUserDonateAsync(u => u.ProjectResource.ProjectID.Equals(projectID) && u.Status == 0);
            if (allUserDonate == null)
            {
                return NotFound();
            }

            return View(allUserDonate);
        }

        [HttpGet]
        [Route("Project/ReviewOrgDonateRequest/{projectID}")]
        public async Task<IActionResult> ReviewOrgDonateRequest(Guid projectID)
        {
            var allOrgDonate =
                await projectRepository.GetAllOrganizationDonateAsync(
                    u => u.ProjectResource.ProjectID.Equals(projectID) && u.Status == 0);
            if (allOrgDonate == null)
            {
                return NotFound();
            }

            return View(allOrgDonate);
        }

        //-----accept request donate-----
        public async Task<IActionResult> AcceptedDonateRequest(Guid transactionID, string donor)
        {
            var res = false;
            switch (donor)
            {
                case "User":
                    res = await projectRepository.AcceptedUserDonateRequestAsync(transactionID);
                    break;
                case "Organization":
                    res = await projectRepository.AcceptedOrgDonateRequestAsync(transactionID);
                    break;
                default:
                    return NotFound();
            }

            if (res)
            {
                TempData[MyConstants.Success] = "Donation request accepted successfully!";
                return (donor.Equals("User"))
                    ? RedirectToAction(nameof(ReviewUserDonateRequest),
                        new { id = HttpContext.Session.GetString("currentProjectID") })
                    : RedirectToAction(nameof(ReviewOrgDonateRequest),
                        new { id = HttpContext.Session.GetString("currentProjectID") });
            }

            TempData[MyConstants.Error] = "Failed to accept the donation request!";
            return (donor.Equals("User"))
                ? RedirectToAction(nameof(ReviewUserDonateRequest),
                    new { id = HttpContext.Session.GetString("currentProjectID") })
                : RedirectToAction(nameof(ReviewOrgDonateRequest),
                    new { id = HttpContext.Session.GetString("currentProjectID") });
        }

        public async Task<IActionResult> AcceptedDonateRequestAll(string donor)
        {
            var currentProjectID = HttpContext.Session.GetString("currentProjectID");
            switch (donor)
            {
                case "User":
                    var allUserDonateRequest =
                        await projectRepository.GetAllUserDonateAsync(
                            u => u.ProjectResource.ProjectID.Equals(new Guid(currentProjectID)) && u.Status == 0);
                    if (allUserDonateRequest == null)
                    {
                        return NotFound();
                    }

                    foreach (var userDonate in allUserDonateRequest)
                    {
                        var res = await projectRepository.AcceptedUserDonateRequestAsync(userDonate.TransactionID);
                        if (!res)
                        {
                            TempData[MyConstants.Error] = "Failed to accept all the donation request!";
                            return RedirectToAction(nameof(ReviewUserDonateRequest),
                                new { id = HttpContext.Session.GetString("currentProjectID") });
                        }
                    }

                    break;
                case "Organization":
                    var allOrgDonateRequest = await projectRepository.GetAllOrganizationDonateAsync(
                        u => u.ProjectResource.ProjectID.Equals(new Guid(currentProjectID)) && u.Status == 0);
                    if (allOrgDonateRequest == null)
                    {
                        return NotFound();
                    }

                    foreach (var orgDonate in allOrgDonateRequest)
                    {
                        var res = await projectRepository.AcceptedUserDonateRequestAsync(orgDonate.TransactionID);
                        if (!res)
                        {
                            TempData[MyConstants.Error] = "Failed to accept all the donation request!";
                            return RedirectToAction(nameof(ReviewOrgDonateRequest),
                                new { id = HttpContext.Session.GetString("currentProjectID") });
                        }
                    }

                    break;
            }

            TempData[MyConstants.Success] = "Donation all request accepted successfully!";
            return (donor.Equals("User"))
                ? RedirectToAction(nameof(ReviewUserDonateRequest),
                    new { id = HttpContext.Session.GetString("currentProjectID") })
                : RedirectToAction(nameof(ReviewOrgDonateRequest),
                    new { id = HttpContext.Session.GetString("currentProjectID") });
        }

        //-----deny request donate-----
        public async Task<IActionResult> DenyDonateRequest(Guid transactionID, string donor)
        {
            var res = false;
            switch (donor)
            {
                case "User":
                    res = await projectRepository.DenyUserDonateRequestAsync(transactionID);
                    break;
                case "Organization":
                    res = await projectRepository.DenyOrgDonateRequestAsync(transactionID);
                    break;
            }

            if (res)
            {
                TempData[MyConstants.Success] = "Donation request denied successfully!";
                return (donor.Equals("User"))
                    ? RedirectToAction(nameof(ReviewUserDonateRequest),
                        new { id = HttpContext.Session.GetString("currentProjectID") })
                    : RedirectToAction(nameof(ReviewOrgDonateRequest),
                        new { id = HttpContext.Session.GetString("currentProjectID") });
            }

            TempData[MyConstants.Error] = "Failed to deny the donation request!";
            return (donor.Equals("User"))
                ? RedirectToAction(nameof(ReviewUserDonateRequest),
                    new { id = HttpContext.Session.GetString("currentProjectID") })
                : RedirectToAction(nameof(ReviewOrgDonateRequest),
                    new { id = HttpContext.Session.GetString("currentProjectID") });
        }

        public async Task<IActionResult> DenyDonateRequestAll(string donor)
        {
            var currentProjectID = HttpContext.Session.GetString("currentProjectID");
            switch (donor)
            {
                case "User":
                    var allUserDonateRequest =
                        await projectRepository.GetAllUserDonateAsync(
                            u => u.ProjectResource.ProjectID.Equals(new Guid(currentProjectID)) && u.Status == 0);
                    if (allUserDonateRequest == null)
                    {
                        return NotFound();
                    }

                    foreach (var userDonate in allUserDonateRequest)
                    {
                        var res = await projectRepository.DenyUserDonateRequestAsync(userDonate.TransactionID);
                        if (!res)
                        {
                            TempData[MyConstants.Error] = "Failed to deny all the donation request!";
                            return RedirectToAction(nameof(ReviewUserDonateRequest),
                                new { id = HttpContext.Session.GetString("currentProjectID") });
                        }
                    }

                    break;
                case "Organization":
                    var allOrgDonateRequest = await projectRepository.GetAllOrganizationDonateAsync(
                        u => u.ProjectResource.ProjectID.Equals(new Guid(currentProjectID)) && u.Status == 0);
                    if (allOrgDonateRequest == null)
                    {
                        return NotFound();
                    }

                    foreach (var orgDonate in allOrgDonateRequest)
                    {
                        var res = await projectRepository.DenyOrgDonateRequestAsync(orgDonate.TransactionID);
                        if (!res)
                        {
                            TempData[MyConstants.Error] = "Failed to deny all the donation request!";
                            return RedirectToAction(nameof(ReviewOrgDonateRequest),
                                new { id = HttpContext.Session.GetString("currentProjectID") });
                        }
                    }

                    break;
            }

            TempData[MyConstants.Success] = "All donation request denied successfully!";
            return (donor.Equals("User"))
                ? RedirectToAction(nameof(ReviewUserDonateRequest),
                    new { id = HttpContext.Session.GetString("currentProjectID") })
                : RedirectToAction(nameof(ReviewOrgDonateRequest),
                    new { id = HttpContext.Session.GetString("currentProjectID") });
        }

        //---------------------------manage ProjectResource--------------------------
        public async Task<IActionResult> CreateProjectDonateRequestAsync(Guid projectID)
        {
            IEnumerable<SelectListItem> ResourceTypeList =  projectRepository.GetAllProjectResourceQuery().Select(x =>
                new SelectListItem
                {
                    Text = x.ResourceName,
                    Value = x.ResourceID.ToString(),
                });
            ResourceTypeList.Append(new SelectListItem() { Text = "Other", Value = "None" });
            ViewData["ResourceTypeList"] = ResourceTypeList;
            return View();
        }

        [Route("Project/ManageProjectResource/{projectID}")]
        public async Task<IActionResult> ManageProjectResource(Guid projectID)
        {
            var allResource = await projectRepository.FilterProjectResourceAsync(p => p.ProjectID.Equals(projectID));
            if (allResource == null)
            {
                return NotFound();
            }

            return View(allResource);
        }

        [HttpPost]
        public async Task<IActionResult> AddProjectResourceType(ProjectResource projectResource)
        {
            if (ModelState.IsValid)
            {
                var projectObj =
                    await projectRepository.GetProjectAsync(p => p.ProjectID.Equals(projectResource.ProjectID));
                if (projectObj != null)
                {
                    //check whether the ProjectResource has same name and same unit is existed
                    //var existingResource = await projectRepository.FilterProjectResourceAsync(p => p.ProjectID.Equals(projectResource.ProjectID) && p.ResourceName.Equals(projectResource.ResourceName) && p.Unit.Equals(projectResource.Unit));
                    //if(existingResource.FirstOrDefault() != null)
                    //{
                    //    return RedirectToAction(nameof(ManageProjectResource), new { id = projectObj.ProjectID });
                    //}

                    var resAddResource = await projectRepository.AddResourceTypeAsync(projectResource);
                    if (resAddResource)
                    {
                        TempData[MyConstants.Success] = "Add resource type successfully!";
                        return RedirectToAction(nameof(ManageProjectResource), new { id = projectObj.ProjectID });
                    }

                    return RedirectToAction(nameof(ManageProjectResource), new { id = projectObj.ProjectID });
                }
            }

            TempData[MyConstants.Error] = "Fail to add new resource type!";
            return RedirectToAction(nameof(ManageProjectResource),
                new { id = HttpContext.Session.GetString("currentProjectID") });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateResourceType(ProjectResource projectResource)
        {
            var projectResourceObj =
                await projectRepository.FilterProjectResourceAsync(p =>
                    p.ResourceID.Equals(projectResource.ResourceID));

            if (projectResourceObj.FirstOrDefault() != null)
            {
                //check whether the resource has same name and same unit is existed
                var existingResource = await projectRepository.FilterProjectResourceAsync(p =>
                p.ResourceID!=projectResource.ResourceID &&
                    p.ProjectID.Equals(projectResource.ProjectID) &&
                    p.ResourceName.Equals(projectResource.ResourceName) && p.Unit.Equals(projectResource.Unit));
                if (existingResource.FirstOrDefault() != null)
                {
                    TempData[MyConstants.Error] = "Resource type has the same unit is existed!";
                    return RedirectToAction(nameof(ManageProjectResource), new { id = projectResource.ProjectID });
                }

                var res = await projectRepository.UpdateResourceTypeAsync(projectResource);
                if (res)
                {
                    TempData[MyConstants.Success] = "Update resource type successfully!";
                    return RedirectToAction(nameof(ManageProjectResource), new { id = projectResource.ProjectID });
                }
            }

            TempData[MyConstants.Error] = "Fail to update resource type!";
            return RedirectToAction(nameof(ManageProjectResource), new { id = projectResource.ProjectID });
        }

        public async Task<IActionResult> DeleteResourceType(Guid resourceID)
        {
            var res = await projectRepository.DeleteResourceTypeAsync(resourceID);
            if (res)
            {
                TempData[MyConstants.Success] = "Delete resource type successfully!";
                return RedirectToAction(nameof(ManageProjectResource),
                    new { id = HttpContext.Session.GetString("currentProjectID") });
            }

            TempData[MyConstants.Error] = "Fail to delete resource type!";
            return RedirectToAction(nameof(ManageProjectResource),
                new { id = HttpContext.Session.GetString("currentProjectID") });
        }

        //-----------------manage project phase report -------------------
        [Route("Project/ManageProjectPhaseReport/{projectID}")]
        public async Task<IActionResult> ManageProjectPhaseReport(Guid projectID)
        {
            var allUpdate =
                await projectRepository.GetAllPhaseReportsAsync(u => u.ProjectID.Equals(projectID));
            if (allUpdate == null)
            {
                return NotFound();
            }

            allUpdate = allUpdate.OrderByDescending(x => x.Date).ToList();
            return View(allUpdate);
        }

        [HttpGet]
        public async Task<IActionResult> AddProjectPhaseReport(Guid projectID)
        {
            var existingDates = await projectRepository.GetExistingReportDatesAsync(projectID);
            // Convert to a list of string dates in "YYYY-MM-DD" format
            var disabledDates = existingDates.Select(d => d.ToString("yyyy-MM-dd")).ToList();
            ViewBag.DisabledDates = disabledDates;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProjectPhaseReport(History history, List<IFormFile> images)
        {
            if (history == null) return NotFound();
            history.HistoryID = new Guid();

            if (images != null && images.Count() > 0)
            {
                var resAttachment = await Util.UploadImages(images, @"images\Project\Phase");
                if (resAttachment.Equals("No file"))
                {
                    TempData[MyConstants.Error] = "No file to upload!";
                    return RedirectToAction(nameof(UpdateProjectProfile), new { id = history.ProjectID });
                }

                if (resAttachment.Equals("Wrong extension"))
                {
                    TempData[MyConstants.Error] = "Extension of some files is wrong!";
                    return RedirectToAction(nameof(UpdateProjectProfile), new { id = history.ProjectID });
                }

                history.Attachment = resAttachment;
            }

            var res = await projectRepository.AddPhaseReportAsync(history);
            if (res)
            {
                TempData[MyConstants.Success] = "Add project update successfully!";
                return RedirectToAction(nameof(ManageProjectPhaseReport), new { id = history.ProjectID });
            }

            TempData[MyConstants.Error] = "Fail to add project update!";
            return RedirectToAction(nameof(ManageProjectPhaseReport), new { id = history.ProjectID });
        }

        public async Task<IActionResult> EditProjectPhaseReport(Guid historyID, Guid projectID)
        {
            var projectUpdates =
                await projectRepository.GetAllPhaseReportsAsync(u => u.HistoryID.Equals(historyID));
            if (projectUpdates == null)
            {
                return NotFound();
            }

            var projectUpdate = projectUpdates.FirstOrDefault();
            var existingDates = await projectRepository.GetExistingReportDatesAsync(projectID);
            // Convert to a list of string dates in "YYYY-MM-DD" format
            var disabledDates = existingDates.Select(d => d.ToString("yyyy-MM-dd"))
                .Except(new[] { projectUpdate?.Date.ToString("yyyy-MM-dd") }).ToList();
            ViewBag.DisabledDates = disabledDates;
            return View(projectUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> EditProjectPhaseReport(History history, List<IFormFile> images)
        {
            if (history == null) return NotFound();
            if (images != null && images.Count() > 0)
            {
                var resAttachment = await Util.UploadImages(images, @"images\Project\Phase");
                if (resAttachment.Equals("No file"))
                {
                    TempData[MyConstants.Error] = "No file to upload!";
                    return RedirectToAction(nameof(ManageProjectPhaseReport), new { id = history.ProjectID });
                }

                if (resAttachment.Equals("Wrong extension"))
                {
                    TempData[MyConstants.Error] = "Extension of some files is wrong!";
                    return RedirectToAction(nameof(ManageProjectPhaseReport), new { id = history.ProjectID });
                }

                history.Attachment = resAttachment;
            }


            var res = await projectRepository.EditPhaseReportAsync(history);
            if (res)
            {
                TempData[MyConstants.Success] = "Update project update successfully!";
                return RedirectToAction(nameof(ManageProjectPhaseReport), new { id = history.ProjectID });
            }

            TempData[MyConstants.Error] = "Fail to update project update!";
            return RedirectToAction(nameof(ManageProjectPhaseReport), new { id = history.ProjectID });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProjectPhaseReport(Guid id)
        {
            var res = await projectRepository.DeletePhaseReportAsync(id);
            if (res)
            {
                TempData[MyConstants.Success] = "Delete project update successfully!";
                return RedirectToAction(nameof(ManageProjectPhaseReport),
                    new { id = new Guid(HttpContext.Session.GetString("currentProjectID")) });
            }

            TempData[MyConstants.Error] = "Fail to delete project update!";
            return RedirectToAction(nameof(ManageProjectPhaseReport),
                new { id = new Guid(HttpContext.Session.GetString("currentProjectID")) });
        }



        public async Task<IActionResult> CreateProjectByImportData(Guid requestId)
        {
            Request request = await requestRepository.GetAsync(r => r.RequestID.Equals(requestId));

            //get current user
            var userString = HttpContext.Session.GetString("user");
            User currentUser = null;
            if (userString != null)
            {
                currentUser = JsonConvert.DeserializeObject<User>(userString);
            }

            var currentOrganization = await _organizationService.GetOrganizationVMByUserIDAsync(currentUser.UserID);

            var projectVM = new ProjectVM()
            {
                ProjectID = Guid.NewGuid(),
                OrganizationID = currentOrganization.OrganizationID,
                ProjectStatus = 0,
                StartTime = DateOnly.FromDateTime(DateTime.UtcNow),
                OrganizationVM = currentOrganization,
                RequestID = requestId,
                Attachment = request.Attachment,
                ProjectName = request.RequestTitle,
                ProjectDescription = request.Content,
                ProjectEmail = request.RequestEmail,
                ProjectPhoneNumber = request.RequestPhoneNumber,
                ProjectAddress = request.Location
            };

            ViewBag.ExpectedQuantity = 0;
            ViewBag.RequestId = requestId;
            return View(projectVM);
        }


        //Repo of tuan
        [HttpGet]
        public async Task<IActionResult> CreateProject(Guid? requestId)
        {
            var currentOrganization = HttpContext.Session.Get<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY);

            var userString = HttpContext.Session.GetString("user");
            User currentUser = null;
            if (userString != null)
            {
                currentUser = JsonConvert.DeserializeObject<User>(userString);
            }

            if (requestId != null)
            {
                currentOrganization = await _organizationService.GetOrganizationVMByUserIDAsync(currentUser.UserID);
            }

            var projectVM = new ProjectVM()
            {
                ProjectID = Guid.NewGuid(),
                OrganizationID = currentOrganization.OrganizationID,
                ProjectStatus = 0,
                StartTime = DateOnly.FromDateTime(DateTime.UtcNow),
                OrganizationVM = currentOrganization,
                RequestID = requestId != Guid.Empty ? requestId : null,
            };

            ViewBag.ExpectedQuantity = 0;
            return View(projectVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectVM projectVM, IFormFile image, int expectedQuantity, string Unit)
        {
            var currentOrganization = HttpContext.Session.Get<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY);

            var userString = HttpContext.Session.GetString("user");
            User currentUser = null;
            if (userString != null)
            {
                currentUser = JsonConvert.DeserializeObject<User>(userString);
            }

            if (currentOrganization == null)
            {
                currentOrganization = await _organizationService.GetOrganizationVMByUserIDAsync(currentUser.UserID);
            }

            projectVM.OrganizationVM = currentOrganization;

            if(projectVM.LeaderID != Guid.Empty)
            {
                var Leader = new User();
                foreach (var item in currentOrganization.OrganizationMember)
                {
                    if (item.UserID.Equals(projectVM.LeaderID))
                    {
                        Leader = item.User;
                    }
                }
                if (projectVM != null)
                {
                    if (image != null)
                    {
                        projectVM.Attachment = Util.UploadImage(image, @"images\Project");
                    }

                    if (projectVM.ProjectEmail == null)
                    {
                        projectVM.ProjectEmail = Leader.UserEmail;
                    }
                    if (projectVM.ProjectPhoneNumber == null)
                    {
                        projectVM.ProjectPhoneNumber = Leader.UserPhoneNumber;
                    }
                    if (projectVM.ProjectAddress == null)
                    {
                        projectVM.ProjectAddress = Leader.UserAddress;
                    }

                    // user not update profile.
                    if (expectedQuantity > 0)
                    {
                        var project = new Models.Models.Project()
                        {
                            ProjectID = projectVM.ProjectID,
                            OrganizationID = projectVM.OrganizationID,
                            RequestID = projectVM.RequestID,
                            ProjectName = projectVM.ProjectName,
                            ProjectEmail = projectVM.ProjectEmail,
                            ProjectPhoneNumber = projectVM.ProjectPhoneNumber,
                            ProjectAddress = projectVM.ProjectAddress,
                            ProjectStatus = projectVM.ProjectStatus,
                            Attachment = projectVM.Attachment,
                            ProjectDescription = projectVM.ProjectDescription,
                            StartTime = projectVM.StartTime,
                            EndTime = projectVM.EndTime,
                        };



                        if (await projectRepository.AddProjectAsync(project))
                        {
                            var projectResource = new ProjectResource()
                            {
                                ProjectID = project.ProjectID,
                                ResourceName = "Money",
                                Quantity = 0,
                                ExpectedQuantity = expectedQuantity,
                                Unit = Unit,
                            };
                            await projectRepository.AddProjectResourceAsync(projectResource);

                            if(project.RequestID != null)
                            {
                                Request request = await requestRepository.GetAsync(r => r.RequestID.Equals(project.RequestID));
                                request.Status = 2;
                                await requestRepository.UpdateAsync(request);
                            }
                           

                            return RedirectToAction(nameof(AutoJoinProject), new { projectId = project.ProjectID, leaderId = projectVM.LeaderID });
                        }
                    }

                }
            }
            

            if(expectedQuantity <= 0) 
                ViewBag.MessageExpectedQuantity = "*ExpectedQuantity more than > 0";

            ViewBag.ExpectedQuantity = expectedQuantity;

            return View(projectVM);

           
        }

        public async Task<IActionResult> AutoJoinProject(Guid projectId, Guid leaderId)
        {
            //get current user
            var userString = HttpContext.Session.GetString("user");
            User currentUser = null;
            if (userString != null)
            {
                currentUser = JsonConvert.DeserializeObject<User>(userString);
            }

            //join Project
            
            if (!currentUser.UserID.Equals(leaderId))
            {
                var projectMember1 = new ProjectMember()
                {
                    UserID = leaderId,
                    ProjectID = projectId,
                    Status = 3,
                };
                await projectRepository.AddProjectMemberAsync(projectMember1);
            }
            var projectMember = new ProjectMember()
            {
                UserID = currentUser.UserID,
                ProjectID = projectId,
                Status = 2,
            };
            await projectRepository.AddProjectMemberAsync(projectMember);
           
            var currentProject = await projectRepository.GetProjectByProjectIDAsync(p => p.ProjectID.Equals(projectId));
            HttpContext.Session.Set<Models.Models.Project>(MySettingSession.SESSION_Current_Project_KEY, currentProject);
            return RedirectToAction(nameof(AddProjectResource));
        }
        [HttpGet]

        public async Task<IActionResult> DetailProject(Guid projectId)
        {
            var currentProject = await projectRepository.GetProjectByProjectIDAsync(p => p.ProjectID.Equals(projectId));
            HttpContext.Session.Set<Models.Models.Project>(MySettingSession.SESSION_Current_Project_KEY, currentProject);
            return RedirectToAction(nameof(AddProjectResource));
        }

        public async Task<IActionResult> AddProjectResource()
        {
            var currentProject = HttpContext.Session.Get<Models.Models.Project>(MySettingSession.SESSION_Current_Project_KEY);

            var projectResource = new ProjectResource()
            {
                ProjectID = currentProject.ProjectID,
                Quantity = 0,
            };

            var projectResources =await projectRepository.GetAllResourceByProjectIDAsync(pr => pr.ProjectID .Equals(currentProject.ProjectID));
            HttpContext.Session.Set<List<ProjectResource>>(MySettingSession.SESSION_Resources_In_A_PRoject_KEY, projectResources);

            return View(projectResource);
        }

        [HttpPost]
        public async Task<IActionResult> AddProjectResource(ProjectResource projectResource)
        {
            await projectRepository.AddProjectResourceAsync(projectResource);
            return RedirectToAction(nameof(AddProjectResource));
        }
    }
}
