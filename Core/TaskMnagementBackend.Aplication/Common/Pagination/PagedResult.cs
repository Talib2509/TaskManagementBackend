using System;
using System.Collections.Generic;

namespace TaskMnagementBackend.Aplication.Common.Pagination
{
    /// <summary>
    /// Səhifələnmiş nəticələr üçün standart wrapper.
    /// </summary>
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();

        public int TotalCount { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;

        public bool HasNextPage => Page < TotalPages;

        public bool HasPreviousPage => Page > 1;

        public static PagedResult<T> Create(List<T> items, int totalCount, int page, int pageSize)
        {
            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
