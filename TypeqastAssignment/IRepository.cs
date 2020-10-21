using System;
using System.Collections.Generic;
using System.Text;

namespace TypeqastAssignment
{
    public interface IRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
    }
}
