using QLSL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLSL.DAL
{
    public class PrimaryWErrorRepository : GenericRepository<PrimaryWError>
    {
        public PrimaryWErrorRepository(QLSLContext context)
            : base(context)
        {
        }


    }
}