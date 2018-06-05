using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static System.String;

namespace promoterplus.serverless.Helpers
{
    public class GeneralHelper
    {
        public static IQueryable Sort(IQueryable collection, string sortBy, string orderBy)
        {
            if (IsNullOrEmpty(sortBy))
            {
                sortBy = "id";
            }
            if (IsNullOrEmpty(orderBy))
            {
                orderBy = "DESC";
            }
            return collection.OrderBy(sortBy + " "+ orderBy);
        }

    }
}
