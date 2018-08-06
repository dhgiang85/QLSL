
using System.Collections.Generic;

using QLSL.Models;

namespace QLSL.DAL
{
    public class ZoneRepository:GenericRepository<Zone>
    {
        public ZoneRepository(QLSLContext context)
            : base(context)
        {
        }

    }
}