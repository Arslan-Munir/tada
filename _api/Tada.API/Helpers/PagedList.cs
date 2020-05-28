using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Tada.API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }

        public PagedList(List<T> items, int totalItems, int currentPage, int itemPerPage)
        {
            TotalItems = totalItems;
            ItemsPerPage = itemPerPage;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(totalItems/ (double)itemPerPage);
            this.AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int currentPage, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, currentPage, pageSize);
        }
    }
}