
using System.Collections.Generic;
using QLSL.Models;

namespace QLSL.DAL
{
    public class InformationVMSRepository:GenericRepository<InformationVMS>
    {
        public InformationVMSRepository(QLSLContext context)
            : base(context)
        {
        }


    }
}