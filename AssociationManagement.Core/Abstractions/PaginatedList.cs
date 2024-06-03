namespace AssociationManagement.Core.Common {
    public class PaginatedList<T> {
        public List<T> Items { get; }
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int TotalRecords { get; }

        public PaginatedList() {
            Items = new List<T>();
            PageIndex = 1;
            TotalPages = 1;
            TotalRecords = 0;
        }
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize) {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalRecords = count;
            Items = items;
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }

}
