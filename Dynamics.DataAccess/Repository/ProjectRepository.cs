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
    }
}
