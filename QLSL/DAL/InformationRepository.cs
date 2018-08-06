
using System.Collections.Generic;
using QLSL.Models;

namespace QLSL.DAL
{
    public class InformationRepository:GenericRepository<Information>
    {
        public InformationRepository(QLSLContext context)
            : base(context)
        {
        }


    }
}