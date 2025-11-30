namespace AspCoreFirstApp.Helpers
{
    public class PaginationHelper
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PaginationHelper(int totalCount, int currentPage, int pageSize = 10)
        {
            TotalCount = totalCount;
            CurrentPage = currentPage < 1 ? 1 : currentPage;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
        }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int StartIndex => (CurrentPage - 1) * PageSize;
    }
}
