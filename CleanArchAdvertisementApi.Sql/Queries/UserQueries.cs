using CleanArchAdvertisementApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchAdvertisementApi.Sql.Queries
{
    public static class UserQueries
    {
        #region ===[ UserQueries Methods ]==================================================
        public static async Task<T> UserById<T>(this IQueryable<T> query, Guid id) where T : User
        {
            return await query.FirstOrDefaultAsync(i => i.Id == id);
        }
        #endregion
    }
}
