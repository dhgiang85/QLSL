using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLSL.Models;

namespace QLSL.DAL
{
    public class TunnelErrorRepository : GenericRepository<TunnelError>
    {
        public TunnelErrorRepository(QLSLContext context)
            : base(context)
        {
        }


    }
    
}