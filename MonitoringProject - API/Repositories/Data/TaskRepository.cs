using MonitoringProject___API.Context;
using MonitoringProject___API.Models;

namespace MonitoringProject___API.Repositories.Data
{
    public class TaskRepository : GeneralRepository<Task, int>
    {
        public TaskRepository(MyContext context) : base(context)
        {
        }
    }
}
