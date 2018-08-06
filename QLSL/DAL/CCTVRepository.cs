using System.Collections.Generic;
using QLSL.Models;

namespace QLSL.DAL
{
    public class CCTVRepository:GenericRepository<CCTV>
    {
        public CCTVRepository(QLSLContext context)
            : base(context)
        {
        }


    }
}