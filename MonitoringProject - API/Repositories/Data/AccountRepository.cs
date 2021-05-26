using MonitoringProject___API.Context;
using MonitoringProject___API.Models;

namespace MonitoringProject___API.Repositories.Data
{
    public class AccountRepository : GeneralRepository<Account, int>
    {
        public AccountRepository(MyContext context) : base(context)
        {
        }
    }
}
