using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskMnagementBackend.Aplication.Common.Pagination
{
    /// <summary>
    /// IQueryable üzərində səhifələmə (paging) və sıralama (sorting) üçün ortaq extension metodlar.
    /// </summary>
    public static class PaginationExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return PagedResult<T>.Create(items, totalCount, page, pageSize);
        }

        /// <summary>
        /// SortBy sahə adına görə (sortMap-də göstərilən expression-lar üzərindən) sıralama tətbiq edir.
        /// SortBy boşdursa və ya tanınmırsa, defaultKey üzrə sıralanır.
        /// </summary>
        public static IQueryable<T> ApplySort<T>(
            this IQueryable<T> query,
            string? sortBy,
            bool desc,
            IReadOnlyDictionary<string, Expression<Func<T, object>>> sortMap,
            string defaultKey)
        {
            var key = string.IsNullOrWhiteSpace(sortBy) ? defaultKey : sortBy.Trim().ToLowerInvariant();

            if (!sortMap.TryGetValue(key, out var selector))
            {
                sortMap.TryGetValue(defaultKey, out selector);
            }

            if (selector is null)
                return query;

            return desc ? query.OrderByDescending(selector) : query.OrderBy(selector);
        }
    }
}
