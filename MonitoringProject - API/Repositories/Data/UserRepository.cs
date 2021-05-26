using MonitoringProject___API.Context;
using MonitoringProject___API.Models;

namespace MonitoringProject___API.Repositories.Data
{
    public class UserRepository : GeneralRepository<User, int>
    {
        public UserRepository(MyContext context) : base(context)
        {
        }
    }
}
