using ClassLibrary1.Domain;
using ClassLibrary1.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace FunctionApp2
{
    public class LocationListResponse
    {
        public int Page { get; internal set; }

        public int PageSize { get; internal set; }

        public bool HasMoreResults { get; internal set; }

        public IEnumerable<Location> Results { get; internal set; }

        public LocationListResponse()
        {
        }

        public LocationListResponse(
            PaginationOptions paginationOptions,
            IEnumerable<Location> locationList)
        {
            this.Page = paginationOptions.Page;
            this.PageSize = paginationOptions.PageSize;
            this.Results = locationList.Take(paginationOptions.PageSize);
            this.HasMoreResults = locationList.Count() > this.PageSize;
        }
    }
}
