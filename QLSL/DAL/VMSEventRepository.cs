
using System.Collections.Generic;

using QLSL.Models;

namespace QLSL.DAL
{
    public class VMSEventRepository: GenericRepository<VMSEvent>
    {
        public VMSEventRepository(QLSLContext context)
            : base(context)
        {
        }


    }
   
}