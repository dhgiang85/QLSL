using System.Collections.Generic;
using QLSL.Models;

namespace QLSL.DAL
{
    public class VMSHistoryStatusRepository : GenericRepository<VMSHistoryStatus>
    {
        public VMSHistoryStatusRepository(QLSLContext context)
            : base(context)
        {
        }


    }
}