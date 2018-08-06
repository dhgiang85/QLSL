using System.Collections.Generic;
using QLSL.Models;

namespace QLSL.DAL
{
    public class AccidentTypeRepository: GenericRepository<AccidentType>
    {
        public AccidentTypeRepository(QLSLContext context)
            : base(context)
        {
        }

        public IEnumerable<TEntity> GetListOfEnrollmentDate<TEntity>(string query)
        {
            return context.Database.SqlQuery<TEntity>(query);
        }
    }
}