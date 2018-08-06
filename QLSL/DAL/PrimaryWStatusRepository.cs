using QLSL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLSL.DAL
{
    public class PrimaryWStatusRepository : GenericRepository<PrimaryWStatus>
    {
        public PrimaryWStatusRepository(QLSLContext context)
            : base(context)
        {
        }


    }
}