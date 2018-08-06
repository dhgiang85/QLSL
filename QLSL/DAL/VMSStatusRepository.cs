using System.Collections.Generic;
using QLSL.Models;

namespace QLSL.DAL
{
    public class VMSStatusRepository: GenericRepository<VMSStatus>
    {
        public VMSStatusRepository(QLSLContext context)
            : base(context)
        {
        }


    }
}