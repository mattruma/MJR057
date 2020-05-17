using FunctionApp2.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace FunctionApp2
{
    public class LocationListResponse
    {
        public int Page { get; internal set; }

        public int PageSize { get; internal set; }

        public bool HasMoreResults { get; internal set; }

        public IEnumerable<LocationListData> Results { get; internal set; }

        public LocationListResponse()
        {
        }

        public LocationListResponse(
            PaginationOptions paginationOptions,
            IEnumerable<LocationListData> locationListDataList)
        {
            this.Page = paginationOptions.Page;
            this.PageSize = paginationOptions.PageSize;
            this.Results = locationListDataList.Take(paginationOptions.PageSize);
            this.HasMoreResults = locationListDataList.Count() > this.PageSize;
        }
    }
}
