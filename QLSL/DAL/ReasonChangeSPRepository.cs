
using System.Collections.Generic;

using QLSL.Models;

namespace QLSL.DAL
{
    public class ReasonChangeSPRepository:GenericRepository<ReasonChangeSP>
    {
        public ReasonChangeSPRepository(QLSLContext context)
            : base(context)
        {
        }

    }
}