using QLSL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLSL.DAL
{
    public class PrimaryWRepository : GenericRepository<PrimaryW>
    {
        public PrimaryWRepository(QLSLContext context)
            : base(context)
        {
        }
    }
}