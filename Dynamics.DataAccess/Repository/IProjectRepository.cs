using Dynamics.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.DataAccess.Repository
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllProjectsAsync();

        Task<bool> AddProjectAsunc(Project entity);


    }
}
