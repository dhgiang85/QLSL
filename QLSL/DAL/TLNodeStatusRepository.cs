using System.Collections.Generic;
using QLSL.Models;

namespace QLSL.DAL
{
    public class TLNodeStatusRepository: GenericRepository<TLNodeStatus>
    {
        public TLNodeStatusRepository(QLSLContext context)
            : base(context)
        {
        }


    }
}