using System.Collections.Generic;
using QLSL.Models;

namespace QLSL.DAL
{
    public class CCTVErrorRepository:GenericRepository<CCTVError>
    {
        public CCTVErrorRepository(QLSLContext context)
            : base(context)
        {
        }


    }
}