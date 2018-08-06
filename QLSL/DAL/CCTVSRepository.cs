
using QLSL.Models;

namespace QLSL.DAL
{
    public class CCTVStatusRepository : GenericRepository<CCTVStatus>
    {
        public CCTVStatusRepository(QLSLContext context)
            : base(context)
        {
        }

    }
}