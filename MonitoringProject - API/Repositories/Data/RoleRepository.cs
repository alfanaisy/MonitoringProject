using MonitoringProject___API.Context;
using MonitoringProject___API.Models;

namespace MonitoringProject___API.Repositories.Data
{
    public class RoleRepository : GeneralRepository<Role, int>
    {
        public RoleRepository(MyContext context) : base(context)
        {
        }
    }
}
