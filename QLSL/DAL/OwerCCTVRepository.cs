
using System.Collections.Generic;
using QLSL.Models;

namespace QLSL.DAL
{
    public class OwerCCTVRepository:GenericRepository<OwerCCTV>
    {
        public OwerCCTVRepository(QLSLContext context)
            : base(context)
        {
        }


    }
}