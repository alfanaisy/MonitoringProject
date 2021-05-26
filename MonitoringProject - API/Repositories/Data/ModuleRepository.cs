using MonitoringProject___API.Context;
using MonitoringProject___API.Models;

namespace MonitoringProject___API.Repositories.Data
{
    public class ModuleRepository : GeneralRepository<Module, int>
    {
        public ModuleRepository(MyContext context) : base(context)
        {
        }
    }
}
