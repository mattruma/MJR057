using System.Collections.Generic;

namespace BlazorApp2.Helpers
{
    public class PagedResponse<T>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public bool HasMoreResults { get; set; }

        public IEnumerable<T> Results { get; set; }

    }
}
