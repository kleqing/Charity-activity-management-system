using System.Linq.Expressions;
using Dynamics.Models.Models;
using Dynamics.Models.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Dynamics.DataAccess.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _accessor;
        private readonly IProjectMemberRepository _projectMemberRepo;
        private readonly IProjectResourceRepository _projectResourceRepo;

        public ProjectRepository(ApplicationDbContext dbContext, 
            IHttpContextAccessor httpContextAccessor,
            IProjectMemberRepository projectMemberRepository,
            IProjectResourceRepository projectResourceRepository)
        {
            this._db = dbContext;
            this._accessor = httpContextAccessor;
            this._projectMemberRepo = projectMemberRepository;
            this._projectResourceRepo = projectResourceRepository;
        }

        //manage project profile
        //get images
        public async Task<string> GetAllImagesAsync(Guid id, string owner)
        {
            var resImgPath = "";
            if (!string.IsNullOrEmpty(owner) && owner.Equals("Project"))
            {
                var projectObj = await _db.Projects.FirstOrDefaultAsync(x => x.ProjectID.Equals(id));
                if (projectObj != null)
                {
                    resImgPath = projectObj.Attachment;
                }
            }
            else if (!string.IsNullOrEmpty(owner) && owner.Equals("Phase"))
            {
                var historyObj = await _db.Histories.FirstOrDefaultAsync(x => x.HistoryID.Equals(id));
                if (historyObj != null)
                {
                    resImgPath = historyObj.Attachment;
                }
            }

            return resImgPath;
        }

        //shut down
        public async Task<bool> ShutdownProjectAsync(ShutdownProjectVM entity)
        {
            var projectObj = _db.Projects.FirstOrDefault(x => x.ProjectID.Equals(entity.ProjectID));
            if (projectObj != null)
            {
                projectObj.ProjectStatus = -1;
                projectObj.ShutdownReason = entity.Reason;
                _db.Projects.Update(projectObj);
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> FinishProjectAsync(FinishProjectVM entity)
        {
            var projectObj = _db.Projects.FirstOrDefault(x => x.ProjectID.Equals(entity.ProjectID));
            if (projectObj != null)
            {
                projectObj.ProjectStatus = 2;
                projectObj.ReportFile = entity.ReportFile;
                _db.Projects.Update(projectObj);
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> SendReportProjectRequestAsync(Report entity)
        {
            if (entity == null) return false;
            entity.ReportID = Guid.NewGuid();
            entity.ReportDate = DateTime.Now;
            await _db.Reports.AddAsync(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>> GetStatisticOfProject(Guid projectID)
        {
            var projectObj = GetProjectAsync(x => x.ProjectID.Equals(projectID)).Result;
            if (projectObj != null)
            {
                var projectResouceMoney = projectObj.ProjectResource.FirstOrDefault(x => x.ResourceName.ToString().Equals("Money") && x.Unit.Equals("VND"));
                if (projectResouceMoney != null)
                {
                    var progressValue = (double)projectResouceMoney.Quantity / projectResouceMoney.ExpectedQuantity *
                                        100;
                    //cal nums contributor
                    var numberOfProjectContributor = _db.UserToProjectTransactionHistories.Include(x=>x.ProjectResource)
                        .Where(x => x.ProjectResource.ProjectID.Equals(projectID) && x.Status == 1)
                        .Select(x => x.UserID)
                        .Distinct().Count();
                    numberOfProjectContributor += _db.OrganizationToProjectTransactionHistory.Include(x=> x.ProjectResource).Include(x => x.OrganizationResource)
                        .Where(x => x.ProjectResource.ProjectID.Equals(projectID) && x.Status == 1)
                        .Select(x => x.OrganizationResource.OrganizationID)
                        .Distinct().Count();
                    var timeLeft = projectObj.EndTime.HasValue
                        ? projectObj.EndTime.Value.ToDateTime(TimeOnly.MinValue) - DateTime.Now
                        : TimeSpan.Zero;
                    if(progressValue.ToString()==null|| numberOfProjectContributor.ToString() == null|| timeLeft.ToString() == null|| projectResouceMoney.ToString() == null)
                    {
                        return null;
                    }
                    List<string> statistic = new List<string>()
                    {
                        projectResouceMoney?.Quantity.ToString(), projectResouceMoney?.ExpectedQuantity.ToString(),
                        progressValue.ToString(), numberOfProjectContributor.ToString(), timeLeft.ToString()
                    };
                    return statistic;
                }
            }

            return null;
        }

        public async Task<List<Project>> GetAllAsync(Expression<Func<Project, bool>>? filter = null)
        {
            // We include these just in case we want to display all stuff
            if (filter != null)
            {
                return await _db.Projects.Include(pr => pr.ProjectResource)
                    .Where(filter)
                    .Include(pr => pr.ProjectMember).ThenInclude(u => u.User)
                    .ToListAsync();
            }
            return await _db.Projects.Include(pr => pr.ProjectResource)
                .Include(pr => pr.ProjectMember).ThenInclude(u => u.User)
                .ToListAsync();
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            IQueryable<Project> projects = _db.Projects.Include(x => x.ProjectMember).ThenInclude(x => x.User)
                .Include(x => x.ProjectResource)
                .Include(x => x.Organization);
            return await projects.ToListAsync();
        }

        //get leader of project
        public async Task<User> GetProjectLeaderAsync(Guid projectID)
        {
            var projectObj = await GetProjectAsync(x=>x.ProjectID.Equals(projectID));
            ProjectMember leaderProjectMembers =  projectObj?.ProjectMember.Where(x=> x.Status == 3).FirstOrDefault();
            //if no leader then leader is the ceo of organization
            if (leaderProjectMembers == null)
            {
                leaderProjectMembers = projectObj?.ProjectMember.Where(x => x.Status == 2).FirstOrDefault();
            }
            if (leaderProjectMembers!=null)
            {
                return leaderProjectMembers?.User;
            }

            return null;
        }


        public Task<Project?> GetProjectAsync(Expression<Func<Project, bool>> filter)
        {
            var project = _db.Projects.Include(x => x.ProjectMember).ThenInclude(x => x.User)
                .Include(x => x.ProjectResource)
                .Include(x => x.Organization)
                .Include(x => x.Request).ThenInclude(x => x.User)
                .Include(x => x.History).Where(filter);
            return project.FirstOrDefaultAsync();
        }
        public async Task<bool> UpdateAsync(Project entity, Guid newProjectLeaderID)
        {
            var existingItem = await GetProjectAsync(u => entity.ProjectID.Equals(u.ProjectID));
            if (existingItem == null)
            {
                return false;
            }
            _db.Entry(existingItem).CurrentValues.SetValues(entity);
            //updating 2 member who is new and old leader of project
            var oldProjectLeaderUser = GetProjectLeaderAsync(entity.ProjectID).Result;
            var oldProjectLeader = _projectMemberRepo.FilterProjectMember(x => x.UserID.Equals(oldProjectLeaderUser.UserID) && x.ProjectID.Equals(entity.ProjectID)).FirstOrDefault();
            var newProjectLeader = await _db.ProjectMembers.FirstOrDefaultAsync(x =>
                x.UserID.Equals(newProjectLeaderID) && x.ProjectID.Equals(entity.ProjectID));
            var ceoOfProjectID = _accessor.HttpContext.Session.GetString("currentProjectCEOID");
            if (oldProjectLeader != null && newProjectLeader != null &&
                !oldProjectLeader.UserID.Equals(newProjectLeader.UserID))
            {
                if (oldProjectLeader.UserID.ToString().Equals(ceoOfProjectID)){
                    oldProjectLeader.Status = 2;
                }
                else
                {
                    oldProjectLeader.Status = 1;
                }                
                _db.ProjectMembers.Update(oldProjectLeader);
                if (newProjectLeader.UserID.ToString().Equals(ceoOfProjectID))
                {
                    newProjectLeader.Status = 2;
                }
                else
                {
                    newProjectLeader.Status = 3;
                }              
                _db.ProjectMembers.Update(newProjectLeader);
            }

            await _db.SaveChangesAsync();
            return true;
        }

        //Repo of Tuấn

        public async Task<bool> AddProjectAsync(Project project)
        {
            try
            {
                _db.Projects.Add(project);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<List<Project>> GetAllProjectsByOrganizationIDAsync(Expression<Func<Project, bool>> filter)
        {
            var projects = await _db.Projects.Where(filter).ToListAsync();
            return projects;
        }

        public async Task<bool> AddProjectMemberAsync(ProjectMember projectMember)
        {
            try
            {
                _db.ProjectMembers.Add(projectMember);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddProjectResourceAsync(ProjectResource projectResource)
        {
            try
            {
                _db.ProjectResources.Add(projectResource);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Project> GetProjectByProjectIDAsync(Expression<Func<Project, bool>> filter)
        {
            var project = await _db.Projects.Where(filter).FirstOrDefaultAsync();
            return project;
        }

        public async Task<List<ProjectResource>> GetAllResourceByProjectIDAsync(Expression<Func<ProjectResource, bool>> filter)
        {
            var projectRsources = await _db.ProjectResources.Where(filter).ToListAsync();
            return projectRsources;
        }

        public async Task<ProjectResource> GetProjectResourceByResourceIDAsync(Expression<Func<ProjectResource, bool>> filter)
        {
            var projectResource = await _db.ProjectResources.Where(filter).FirstOrDefaultAsync();
            return projectResource;
        }

        public async Task<bool> UpdateProjectResource(ProjectResource entity)
        {
            var projectResourceItem = await GetProjectResourceByResourceIDAsync(p => p.ResourceID.Equals(entity.ResourceID));

            if (projectResourceItem == null)
            {
                return false;
            }
            _db.Entry(projectResourceItem).CurrentValues.SetValues(entity);
            await _db.SaveChangesAsync();
            return true;
        }

    }
}
