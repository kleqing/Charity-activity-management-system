using System.Linq.Expressions;
using Dynamics.Models.Models;
using Dynamics.Models.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Dynamics.DataAccess.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ProjectRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this._db = dbContext;
            this.httpContextAccessor = httpContextAccessor;
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

        public async Task<List<Project>> GetAllAsync()
        {
            return await _db.Projects.Include(pr => pr.ProjectResource)
                .Include(pr => pr.ProjectMember).ThenInclude(u => u.User)
                .ToListAsync();
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            IQueryable<Project> projects = _db.Projects.Include(x => x.ProjectMember).ThenInclude(x => x.User)
                .Include(x => x.ProjectResource)
                .Include(x => x.Organization)
                .Include(x => x.Request).ThenInclude(x => x.User)
                .Include(x => x.History);
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

            existingItem.ProjectName = entity.ProjectName;
            existingItem.ProjectDescription = entity.ProjectDescription;
            existingItem.Attachment = entity.Attachment;
            existingItem.ProjectAddress = entity.ProjectAddress;
            existingItem.ProjectEmail = entity.ProjectEmail;
            existingItem.ProjectPhoneNumber = entity.ProjectPhoneNumber;
            existingItem.StartTime = entity.StartTime;
            existingItem.EndTime = entity.EndTime;
            _db.Projects.Update(existingItem);
            //updating 2 member who is new and old leader of project
            var oldProjectLeaderUser = GetProjectLeaderAsync(entity.ProjectID).Result;
            var oldProjectLeader = FilterProjectMember(x => x.UserID.Equals(oldProjectLeaderUser.UserID) && x.ProjectID.Equals(entity.ProjectID)).FirstOrDefault();
            var newProjectLeader = await _db.ProjectMembers.FirstOrDefaultAsync(x =>
                x.UserID.Equals(newProjectLeaderID) && x.ProjectID.Equals(entity.ProjectID));
            var ceoOfProjectID = httpContextAccessor.HttpContext.Session.GetString("currentProjectCEOID");
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

        //----manage project's member---------------
        public List<ProjectMember> FilterProjectMember(Expression<Func<ProjectMember, bool>> filter)
        {
            IQueryable<ProjectMember> listProjectMember = _db.ProjectMembers.Include(x=>x.User).Where(filter);
            if (listProjectMember!=null)
            {
                return listProjectMember.ToList();
            }

            return null;
        }

        public async Task<List<User>> GetAllMemberOfProjectIDAsync(Guid projectID)
        {
            Project project = await GetProjectAsync(x => x.ProjectID.Equals(projectID));
            if (project != null)
            {
                project.ProjectMember = _db.ProjectMembers.Where(x => x.ProjectID.Equals(projectID) && x.Status == 1).ToList();
                List<User> members = new List<User>();
                if (project != null)
                {
                    foreach (var u in project.ProjectMember)
                    {
                        members.Add(u.User);
                    }
                }
                return members;
            }

            return null;
        }

        public async Task<bool> DeleteMemberProjectByIdAsync(Guid memberID, Guid projectID)
        {
            var memberObj =
                _db.ProjectMembers.FirstOrDefault(x => x.UserID.Equals(memberID) && x.ProjectID.Equals(projectID));
            try
            {
                if (memberObj != null)
                {
                    _db.ProjectMembers.Remove(memberObj);
                    await _db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //------manage member request------------------
        public List<User> FilterMemberOfProject(Expression<Func<ProjectMember, bool>> filter)
        {
            IQueryable<ProjectMember> projectMemberList = _db.ProjectMembers.Include(x=>x.User).Include(x=>x.Project).Where(filter);
            List<User> members = new List<User>();
            if (members != null)
            {
                foreach (var u in projectMemberList)
                {
                    members.Add(u.User);
                }

                return members;
            }

            return null;
        }

        public async Task<bool> AddJoinRequest(Guid memberID, Guid projectID)
        {
            var res = await _db.ProjectMembers.AddAsync(new ProjectMember
                { UserID = memberID, ProjectID = projectID, Status = 0 });
            await _db.SaveChangesAsync();
            return res != null;
        }

        public async Task<bool> AcceptedJoinRequestAsync(Guid memberID, Guid projectID)
        {
            var memberObj =
                await _db.ProjectMembers.FirstOrDefaultAsync(x =>
                    x.UserID.Equals(memberID) && x.ProjectID.Equals(projectID));
            if (memberObj != null)
            {
                memberObj.Status = 1;
                _db.ProjectMembers.Update(memberObj);
                _db.Entry(memberObj).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DenyJoinRequestAsync(Guid memberID, Guid projectID)
        {
            var memberObj =
                await _db.ProjectMembers.FirstOrDefaultAsync(x =>
                    x.UserID.Equals(memberID) && x.ProjectID.Equals(projectID));
            if (memberObj != null)
            {
                _db.ProjectMembers.Remove(memberObj);
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }
        //-----manage resource's statistic----------------
        public IQueryable<ProjectResource> GetAllProjectResourceQuery()
        {
            return  _db.ProjectResources;
        }
        public async Task<bool> HandleResourceAutomatic(Guid transactionID, string donor)
        {
            //take list of resource type in ProjectResource table
            List<Guid> resourceTypes = _db.ProjectResources.Select(x => x.ResourceID).Distinct().ToList();
            //get transaction obj to take the resource name of transaction
            if (!string.IsNullOrEmpty(donor) && donor.Equals("User"))
            {
                var transactionObj = await _db.UserToProjectTransactionHistories.Include(x=>x.ProjectResource).ThenInclude(x=>x.Project)
                    .Where(x => x.TransactionID.Equals(transactionID)).FirstOrDefaultAsync();

                if (transactionObj != null)
                {
                    //check if the resource id of transaction is in the list of resource, if yes, update the quantity of resource
                    if (transactionObj.ProjectResourceID != null && resourceTypes.Contains(transactionObj.ProjectResourceID))
                    {
                        //find the obj resource in ProjectResource table
                        var resourceObj = transactionObj.ProjectResource;
                        if (resourceObj != null)
                        {
                            //update quantity of obj resource
                            resourceObj.Quantity += transactionObj.Amount;
                            _db.ProjectResources.Update(resourceObj);
                            await _db.SaveChangesAsync();
                            return true;
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(donor) && donor.Equals("Organization"))
            {
                var transactionObj =
                    await _db.OrganizationToProjectTransactionHistory.Include(x=>x.ProjectResource).ThenInclude(x=>x.Project)
                    .Include(x=>x.OrganizationResource).ThenInclude(x=>x.Organization)
                    .FirstOrDefaultAsync(x =>
                        x.TransactionID.Equals(transactionID));
                if (transactionObj != null)
                {
                    //check if the resource name of transaction is in the list of resource type, if yes, update the quantity of resource
                    if (transactionObj.ProjectResourceID.HasValue && resourceTypes.Contains(transactionObj.ProjectResourceID.Value))
                    {
                        //find the obj resource in ProjectResource table
                        var resourceObj = transactionObj.ProjectResource;
                        if (resourceObj != null)
                        {
                            //update quantity of obj resource
                            resourceObj.Quantity += transactionObj.Amount;
                            _db.ProjectResources.Update(resourceObj);
                            await _db.SaveChangesAsync();
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public async Task<IEnumerable<ProjectResource?>> FilterProjectResourceAsync(
            Expression<Func<ProjectResource, bool>> filter)
        {
            IQueryable<ProjectResource?> projectResourceList = _db.ProjectResources.Where(filter);
            return await projectResourceList.ToListAsync();
        }

        public async Task<bool> AddResourceTypeAsync(ProjectResource entity)
        {
            if (entity != null)
            {
                entity.ResourceID = Guid.NewGuid();
                entity.Quantity = 0;
                _db.ProjectResources.Add(entity);
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateResourceTypeAsync(ProjectResource entity)
        {
            if (entity == null)
                return false;

            var existingItems = await FilterProjectResourceAsync(u => entity.ResourceID.Equals(u.ResourceID));
            var existingItem = existingItems.FirstOrDefault();

            if (existingItem != null)
            {
                // Update the existing tracked entity properties
                existingItem.ResourceName = entity.ResourceName;
                existingItem.ExpectedQuantity = entity.ExpectedQuantity;
                existingItem.Unit = entity.Unit;

                // No need to set the state or call Update again, since existingItem is already tracked
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteResourceTypeAsync(Guid resourceID)
        {
            var deleteItem = await _db.ProjectResources.FirstOrDefaultAsync(x => x.ResourceID.Equals(resourceID));
            var existUserDonate =
                await _db.UserToProjectTransactionHistories.FirstOrDefaultAsync(x => x.ProjectResourceID.Equals(resourceID));
            var existOrgDonate =
                await _db.OrganizationToProjectTransactionHistory.FirstOrDefaultAsync(x =>
                    x.ProjectResourceID.Equals(resourceID));
            if (deleteItem == null || existUserDonate != null || existOrgDonate != null)
            {
                return false;
            }

            _db.ProjectResources.Remove(deleteItem);
            await _db.SaveChangesAsync();
            return true;
        }


        //-----------------------manage transaction history---------------------------
        public async Task<List<UserToProjectTransactionHistory>> GetRandom5Donors(Guid projectID)
        {
            var userDonate = _db.UserToProjectTransactionHistories.Include(x=>x.User).Include(x=> x.ProjectResource).ThenInclude(x => x.Project)
                .Where(x => x.ProjectResource.ProjectID.Equals(projectID) && x.Status == 1)
                .OrderBy(x => Guid.NewGuid())
                .Take(5)
                .ToList();
            return userDonate;
        }

        public async Task<bool> AddUserDonateRequestAsync(UserToProjectTransactionHistory? userDonate)
        {
            if (userDonate != null)
            {
                userDonate.TransactionID = Guid.NewGuid();
                if (userDonate.Amount <= 0)
                {
                    userDonate.Amount = 1;
                }
                
                userDonate.Status = 0;
                userDonate.Time = DateOnly.FromDateTime(DateTime.Now);
                await _db.UserToProjectTransactionHistories.AddAsync(userDonate);
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> AddOrgDonateRequestAsync(OrganizationToProjectHistory? orgDonate)
        {
            if (orgDonate != null)
            {
                orgDonate.TransactionID = Guid.NewGuid();
                if (orgDonate.Amount <= 0)
                {
                    orgDonate.Amount = 1;
                }

                orgDonate.Status = 0;
                orgDonate.Time = DateOnly.FromDateTime(DateTime.Now);
                await _db.OrganizationToProjectTransactionHistory.AddAsync(orgDonate);
                //find orgresource
                var orgResource = await _db.OrganizationResources.FirstOrDefaultAsync(x =>x.ResourceID==orgDonate.OrganizationResourceID);
                //update value orgresource
                if (orgResource != null) {
                    orgResource.Quantity -= orgDonate.Amount;
                    _db.OrganizationResources.Update(orgResource);
                }
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<UserToProjectTransactionHistory>> GetAllUserDonateAsync(
            Expression<Func<UserToProjectTransactionHistory, bool>> filter)
        {
            IQueryable<UserToProjectTransactionHistory> listUserDonate =
                _db.UserToProjectTransactionHistories.Include(x=>x.ProjectResource).ThenInclude(x=>x.Project).Include(x=>x.User).Where(filter);
            if (listUserDonate!=null)
            {
                return await listUserDonate.ToListAsync();
            }
            return null;
        }

        public async Task<List<OrganizationToProjectHistory>> GetAllOrganizationDonateAsync(
            Expression<Func<OrganizationToProjectHistory, bool>> filter)
        {
            IQueryable<OrganizationToProjectHistory> listOrganizationDonate =
                _db.OrganizationToProjectTransactionHistory.Include(x=>x.ProjectResource).ThenInclude(x=>x.Project).Include(x=>x.OrganizationResource).ThenInclude(X=>X.Organization).Where(filter);
            if (listOrganizationDonate!=null)
            {
                return await listOrganizationDonate.ToListAsync();
            }

            return null;
        }

        //------review donate request------------------
        public async Task<bool> AcceptedUserDonateRequestAsync(Guid transactionID)
        {
            var transactionObj =
                await _db.UserToProjectTransactionHistories.FirstOrDefaultAsync(x =>
                    x.TransactionID.Equals(transactionID));
            if (transactionObj != null)
            {
                //change status of transaction
                transactionObj.Status = 1;
                _db.UserToProjectTransactionHistories.Update(transactionObj);
                await _db.SaveChangesAsync();

                //modify resource of project
                var addResourceAutomatic = HandleResourceAutomatic(transactionID, "User");
                if (addResourceAutomatic.Result)
                    return true;
                return false;
            }

            return false;
        }

        public async Task<bool> DenyUserDonateRequestAsync(Guid transactionID)
        {
            var transactionObj =
                await _db.UserToProjectTransactionHistories.FirstOrDefaultAsync(x =>
                    x.TransactionID.Equals(transactionID));
            if (transactionObj != null)
            {
                _db.UserToProjectTransactionHistories.Remove(transactionObj);
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> AcceptedOrgDonateRequestAsync(Guid transactionID)
        {
            var transactionObj =
                await _db.OrganizationToProjectTransactionHistory.FirstOrDefaultAsync(x =>
                    x.TransactionID.Equals(transactionID));
            if (transactionObj != null)
            {
                //change status of transaction
                transactionObj.Status = 1;
                _db.OrganizationToProjectTransactionHistory.Update(transactionObj);
                await _db.SaveChangesAsync();

                //modify resource of project
                var addResourceAutomatic = HandleResourceAutomatic(transactionID, "Organization");
                if (addResourceAutomatic.Result)
                    return true;
                return false;
            }

            return false;
        }

        public async Task<bool> DenyOrgDonateRequestAsync(Guid transactionID)
        {
            var transactionObj =
                await _db.OrganizationToProjectTransactionHistory.FirstOrDefaultAsync(x =>
                    x.TransactionID.Equals(transactionID));        
            if (transactionObj != null)
            {
                //find orgresource
                var orgResource = await _db.OrganizationResources.FirstOrDefaultAsync(x => x.ResourceID == transactionObj.OrganizationResourceID);
                ;
                //update value orgresource
                if (orgResource != null)
                {
                    orgResource.Quantity += transactionObj.Amount;
                    _db.OrganizationResources.Update(orgResource);
                    _db.OrganizationToProjectTransactionHistory.Remove(transactionObj);
                    await _db.SaveChangesAsync();
                    return true;

                }
            }

            return false;
        }

        //-----------------manage project update----------------------------
        public async Task<List<History>?> GetAllPhaseReportsAsync(Expression<Func<History, bool>> filter)
        {
            IQueryable<History> updates = _db.Histories.Include(x=>x.Project).Where(filter);
            if (updates!=null)
            {
                return await updates.ToListAsync();

            }
            return null;
        }

        public async Task<bool> AddPhaseReportAsync(History entity)
        {
            if (entity == null) return false;

            await _db.Histories.AddAsync(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditPhaseReportAsync(History entity)
        {
            if (entity == null) return false;
            var existingItem = await _db.Histories.FirstOrDefaultAsync(x => x.HistoryID.Equals(entity.HistoryID));
            if (existingItem == null)
            {
                return false;
            }

            // Detach the entity from the context if it's already being tracked
            _db.Entry(existingItem).State = EntityState.Detached;

            // Now update the existing entity with new values
            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePhaseReportAsync(Guid id)
        {
            var deleteItem = await _db.Histories.FirstOrDefaultAsync(x => x.HistoryID.Equals(id));
            if (deleteItem == null)
            {
                return false;
            }

            _db.Histories.Remove(deleteItem);
            await _db.SaveChangesAsync();
            return true;
        }

        //add constraint for project update
        public async Task<List<DateTime>> GetExistingReportDatesAsync(Guid projectID)
        {
            // Retrieve the list of dates where a report exists for the given project
            return await _db.Histories
                .Where(x => x.ProjectID == projectID)
                .Select(x => x.Date.ToDateTime(TimeOnly.MinValue)) // Adjust depending on your column type
                .ToListAsync();
        }

        public async Task<DateTime> GetReportDateAsync(Guid projectID, Guid historyID)
        {
            var history =
                await _db.Histories.FirstOrDefaultAsync(x =>
                    x.ProjectID.Equals(projectID) && x.HistoryID.Equals(historyID));
            if (history != null)
            {
                return history.Date.ToDateTime(TimeOnly.MinValue);
            }

            return DateTime.MinValue;
        }

        public async Task<int> CountMembersOfProjectByIdAsync(Guid projectId)
        {
            return await _db.ProjectMembers.CountAsync(p => p.ProjectID == projectId);
        }
    }
}