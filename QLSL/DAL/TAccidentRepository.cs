

using System.Collections.Generic;
using QLSL.Models;

namespace QLSL.DAL
{
    public class TAccidentRepository: GenericRepository<TAccident>
    {
        public TAccidentRepository(QLSLContext context)
            : base(context)
        {
        }

        //public IEnumerable<TEntity> GetListOfEnrollmentDate<TEntity>(string query)
        //{
        //    return context.Database.SqlQuery<TEntity>(query);
        //}
    
    }
}