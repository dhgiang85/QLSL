
using System.Collections.Generic;

using QLSL.Models;

namespace QLSL.DAL
{
    public class TLNodeHitoryStatusRepository: GenericRepository<TLNodeHitoryStatus>
    {
        public TLNodeHitoryStatusRepository(QLSLContext context)
            : base(context)
        {
        }


    }
}