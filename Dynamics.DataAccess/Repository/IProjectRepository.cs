using Dynamics.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.DataAccess.Repository
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllProjectsByOrganizationIDAsync(Expression<Func<Project, bool>> filter);

        Task<bool> AddProjectAsync(Project entity);

        Task<bool> AddProjectMemberAsync(ProjectMember entity);

        Task<bool> AddProjectResourceAsync(ProjectResource entity);
        Task<Project> GetProjectByProjectIDAsync(Expression<Func<Project, bool>> filter);
    }
}
