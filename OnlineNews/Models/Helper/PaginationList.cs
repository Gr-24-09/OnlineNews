namespace OnlineNews.Models.Helper
{
    public class PaginationList<T> : List<T>
    {
        
            public PaginationList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
            {
                this.AddRange(items);
                TotalCount = count;
                PageIndex = pageIndex;
                PageSize = pageSize;
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            }

            public int TotalCount { get; private set; }
            public int PageIndex { get; private set; }
            public int PageSize { get; private set; }
            public int TotalPages { get; private set; }

            // Check if there's a next page
            public bool HasNextPage => PageIndex < TotalPages;

            // Check if there's a previous page
            public bool HasPreviousPage => PageIndex > 1;

            // Get the next page number
            public int NextPage => HasNextPage ? PageIndex + 1 : TotalPages;

            // Get the previous page number
            public int PreviousPage => HasPreviousPage ? PageIndex - 1 : 1;
        

    }
}
