using MonitoringProject___API.Context;
using MonitoringProject___API.Models;

namespace MonitoringProject___API.Repositories.Data
{
    public class ProjectRepository : GeneralRepository<Project, int>
    {
        public ProjectRepository(MyContext context) : base(context)
        {
        }
    }
}
