using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchAdvertisementApi.Infrastructure.Repository
{
    public static class Pagination
    {
        #region ===[ Pagination Methods ]==================================================
        public static IQueryable<T> PaginationAdv<T>(this IQueryable<T> query, int page, int count)
        {
            return query.Skip(count * (page - 1))
                .Take(count);
        }
        #endregion
    }
}
