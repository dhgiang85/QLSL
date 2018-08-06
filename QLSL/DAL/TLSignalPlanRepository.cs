using System.Collections.Generic;
using QLSL.Models;

namespace QLSL.DAL
{
    public class TLSignalPlanRepository : GenericRepository<TLSignalPlan>
    {
        public TLSignalPlanRepository(QLSLContext context)
            : base(context)
        {
        }


    }
}