
using QLSL.Models;

namespace QLSL.DAL
{
    public class CAMTypeRepository : GenericRepository<CAMType>
    {
        public CAMTypeRepository(QLSLContext context)
            : base(context)
        {
        }

   
    }
}