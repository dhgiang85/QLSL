using QLSL.Models;
using System.Collections.Generic;
namespace QLSL.DAL
{
    public class TLNodeRepository : GenericRepository<TLNode>
    {
        public TLNodeRepository(QLSLContext context)
            : base(context)
        {
        }


    }
}