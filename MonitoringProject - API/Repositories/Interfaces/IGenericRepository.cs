using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringProject___API.Repositories.Interfaces
{
    public interface IGenericRepository<Entity, TId> where Entity : class
    {
        IEnumerable<Entity> GetAll();
        Entity GetById(TId Id);
        int Post(Entity obj);
        int Put(Entity obj);
        int Delete(TId Id);
    }
}
