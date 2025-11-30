namespace AspCoreFirstApp.ViewModels
{
    using AspCoreFirstApp.Helpers;
    using System.Collections.Generic;


    public class PaginatedListViewModel<T>
    {
        public List<T> Items { get; set; }
        public PaginationHelper Pagination { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public List<AspCoreFirstApp.Models.Genre> Genres { get; set; }

        public PaginatedListViewModel()
        {
            Items = new List<T>();
            SortBy = "Id";
            SortOrder = "asc";
        }

        public PaginatedListViewModel(List<T> items, PaginationHelper pagination, string sortBy = "Id", string sortOrder = "asc")
        {
            Items = items;
            Pagination = pagination;
            SortBy = sortBy;
            SortOrder = sortOrder;
        }
    }
}
