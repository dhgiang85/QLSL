using System.Collections.Generic;
using QLSL.Models;

namespace QLSL.DAL
{
    public class VMSRepository : GenericRepository<VMS>
    {
        public VMSRepository(QLSLContext context)
            : base(context)
        {
        }


    }
}