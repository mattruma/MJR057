using ClassLibrary1.Domain;
using ClassLibrary1.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace FunctionApp2
{
    public class OrderListResponse
    {
        public int Page { get; internal set; }

        public int PageSize { get; internal set; }

        public bool HasMoreResults { get; internal set; }

        public IEnumerable<Order> Results { get; internal set; }

        public OrderListResponse()
        {
        }

        public OrderListResponse(
            PaginationOptions paginationOptions,
            IEnumerable<Order> orderList)
        {
            this.Page = paginationOptions.Page;
            this.PageSize = paginationOptions.PageSize;
            this.Results = orderList.Take(paginationOptions.PageSize);
            this.HasMoreResults = orderList.Count() > this.PageSize;
        }
    }
}
