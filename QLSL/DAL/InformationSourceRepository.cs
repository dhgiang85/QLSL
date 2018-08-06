
using System.Collections.Generic;
using QLSL.Models;

namespace QLSL.DAL
{
    public class InformationSourceRepository:GenericRepository<InformationSource>
    {
        public InformationSourceRepository(QLSLContext context)
            : base(context)
        {
        }


    }
}