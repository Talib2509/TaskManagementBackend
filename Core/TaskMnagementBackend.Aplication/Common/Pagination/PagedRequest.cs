using System;

namespace TaskMnagementBackend.Aplication.Common.Pagination
{
    /// <summary>
    /// Səhifələnən (paged) bütün Query Request-lərinin əsas sinfi.
    /// </summary>
    public abstract class PagedRequest
    {
        private int _page = 1;
        private int _pageSize = 20;

        public int Page
        {
            get => _page;
            set => _page = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 20 : value > 200 ? 200 : value;
        }

        /// <summary>
        /// Sıralama üçün sahə adı (case-insensitive). Boşdursa handler-in default sort-u tətbiq olunur.
        /// </summary>
        public string? SortBy { get; set; }

        /// <summary>
        /// true olarsa azalan (descending), əks halda artan (ascending) sıralama.
        /// </summary>
        public bool Desc { get; set; } = false;
    }
}
