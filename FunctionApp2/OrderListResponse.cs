using ClassLibrary1;
using System.Collections.Generic;
using System.Linq;

namespace FunctionApp2
{
    public class OrderListResponse
    {
        public int Page { get; internal set; }

        public int PageSize { get; internal set; }

        public bool HasMoreResults { get; internal set; }

        public IEnumerable<OrderListData> Results { get; internal set; }

        public OrderListResponse()
        {
        }

        public OrderListResponse(
            PaginationOptions paginationOptions,
            IEnumerable<OrderListData> orderListDataList)
        {
            this.Page = paginationOptions.Page;
            this.PageSize = paginationOptions.PageSize;
            this.Results = orderListDataList.Take(paginationOptions.PageSize);
            this.HasMoreResults = orderListDataList.Count() > this.PageSize;
        }
    }
}
