using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Helper
{
    public class PagedList<T> : List<T>
    {
        public int Total_pages { get; set; }
        public int Page_index { get; set; }
        public int Total_count { get; set; }

        public PagedList(IEnumerable<T> items, int count, int page_index, int page_size)
        {
            Total_count = count;
            Page_index = page_index;
            Total_pages = (int)Math.Ceiling(count / (double)page_size);
            AddRange(items);
        }

        public bool Prev
        {
            get
            {
                return (Page_index > 0);
            }
        }

        public bool Next
        {
            get
            {
                return (Page_index < (Total_pages - 1));
            }
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source,
            int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
