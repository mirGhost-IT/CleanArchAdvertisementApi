using CleanArchAdvertisementApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CleanArchAdvertisementApi.Sql.Queries
{
    public static class AdvertisementQueries
    {
        #region ===[ AdvertisementQueries Methods ]==================================================
        public static async Task<List<T>> AdvertisementAll<T>(this IQueryable<T> query)
        {
            return await query.Select(i => i).ToListAsync();
        }
        public static async Task<T> AdvertisementById<T>(this IQueryable<T> query, Guid id) where T : Advertisement
        {
            return await query.FirstOrDefaultAsync(i => i.Id == id);
        }
        public static async Task<List<T>> AdvertisementMultiSort<T>(
            this IQueryable<T> query,
            string? search,
            string? orderByQueryString,
            DateTime? startDate,
            DateTime? endDate
            ) where T : Advertisement
        {
            if (search != null)
            {
                query = query.Where(i => i.Text.ToLower().Contains(search.ToLower())
                    || i.Rating.ToString().ToLower().Contains(search.ToLower())
                    || i.User.Name.ToLower().Contains(search.ToLower())
                    || i.Created.Date.ToString().ToLower().Contains(search.ToLower())
                    || i.Number.ToString().ToLower().Contains(search.ToLower()));
            }

            if (orderByQueryString != null)
            {
                var orderParams = orderByQueryString.Trim().Split(',');
                var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var orderQueryBuilder = new StringBuilder();

                foreach (var param in orderParams)
                {
                    if (string.IsNullOrWhiteSpace(param))
                        continue;

                    var propertyFromQueryName = param.Split(" ")[0];
                    var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                    if (objectProperty == null)
                        continue;

                    var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

                    orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
                }

                var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

                query = query.OrderBy(orderQuery);
            }

            if (startDate != null && endDate != null)
            {
                try
                {
                    query = query.Where(i => i.Created <= DateTime.Parse(endDate.ToString()).ToUniversalTime() && i.Created >= DateTime.Parse(startDate.ToString()).ToUniversalTime());
                }
                catch (Exception)
                {
                }
            }
            return await query.ToListAsync();
        }
        #endregion
    }
}
