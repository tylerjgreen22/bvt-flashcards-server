namespace Application.Core
{
    // The paging params that are able to be passed via query parameters. Contains the information for pagination, as well as searching, sorting and filtering
    public class PagingParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;

        // If the provided page size is greater than the max page size, the page size will be set to max page size
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string Search { get; set; }
        public string OrderBy { get; set; }
        public bool ByUser { get; set; }
    }
}