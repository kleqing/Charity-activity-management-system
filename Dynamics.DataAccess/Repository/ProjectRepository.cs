using Dynamics.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.DataAccess.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _db;

        public ProjectRepository(ApplicationDbContext db)
        {
            _db = db;
        }

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
            var projectItem = await GetProjectResourceByResourceIDAsync(p => p.ProjectID.Equals(entity.ProjectID));

            if (projectItem == null)
            {
                return false;
            }
            _db.Entry(projectItem).CurrentValues.SetValues(entity);
            await _db.SaveChangesAsync();
            return true;
        }

    }
}
