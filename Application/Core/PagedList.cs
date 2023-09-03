using Microsoft.EntityFrameworkCore;

namespace Application.Core
{
    // Creates a paged list from an incoming list of items
    public class PagedList<T> : List<T>
    {
        // Constructor that takes the list of items, as well as the count page num and page size and creates a list
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        // Method to take a queryable, get the count and items from the DB using the query, and then use the constructor to create a paged list using the provided arguments
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber,
        int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}