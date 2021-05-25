using MonitoringProject___API.Context;
using MonitoringProject___API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringProject___API.Repositories.Data
{
    public class RoleRepository : GeneralRepository<Role, int>
    {
        public RoleRepository(MyContext context) : base(context)
        {
        }
    }
}
