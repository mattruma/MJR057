using FunctionApp2.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace FunctionApp2
{
    public class Function4Response
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public bool HasMoreResults { get; set; }

        public IEnumerable<Function4Data> Results { get; set; }

        public Function4Response()
        {
        }

        public Function4Response(
            PaginationOptions paginationOptions,
            IEnumerable<Function4Data> function4DataList)
        {
            this.Page = paginationOptions.Page;
            this.PageSize = paginationOptions.PageSize;
            this.Results = function4DataList.Take(paginationOptions.PageSize);
            this.HasMoreResults = function4DataList.Count() > this.PageSize;
        }
    }
}
