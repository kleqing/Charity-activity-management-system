using System.Linq.Expressions;
using Dynamics.Models.Models;
using Dynamics.Models.Models.ViewModel;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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
        // Use for display purposes - Kiet
        Task<List<Project>> GetAllProjectsAsync();

        //using this to get project member of leader
        Task<User> GetProjectLeaderAsync(Guid projectID);

        //using this to view detail project
        Task<List<string>> GetStatisticOfProject(Guid projectID);
        Task<Project?> GetProjectAsync(Expression<Func<Project, bool>> filter);
        Task<bool> UpdateAsync(Project entity, Guid newProjectLeaderID);
        Task<bool> ShutdownProjectAsync(ShutdownProjectVM entity);
        Task<bool> FinishProjectAsync(FinishProjectVM entity);

        Task<bool> SendReportProjectRequestAsync(Report entity);

        //manage images of project/phase

        Task<string> GetAllImagesAsync(Guid id, string owner);

      

   


        //Repose ò tuan
        Task<List<Project>> GetAllProjectsByOrganizationIDAsync(Expression<Func<Project, bool>> filter);

        Task<bool> AddProjectAsync(Project entity);

        Task<bool> AddProjectMemberAsync(ProjectMember entity);

        Task<bool> AddProjectResourceAsync(ProjectResource entity);
        Task<Project> GetProjectByProjectIDAsync(Expression<Func<Project, bool>> filter);

        Task<List<ProjectResource>> GetAllResourceByProjectIDAsync(Expression<Func<ProjectResource, bool>> filter);

        Task<bool> UpdateProjectResource(ProjectResource entity);
        //repose of kiet
        Task<List<Project>> GetAllAsync(Expression<Func<Project, bool>>? filter = null);
    }
}
