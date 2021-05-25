using MonitoringProject___API.Context;
using MonitoringProject___API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringProject___API.Repositories.Data
{
    public class UserRepository : GeneralRepository<User, int>
    {
        public UserRepository(MyContext context) : base(context)
        {
        }
    }
}
