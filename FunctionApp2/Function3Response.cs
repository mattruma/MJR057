using FunctionApp2.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace FunctionApp2
{
    public class Function3Response
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public bool HasMoreResults { get; set; }

        public IEnumerable<Function3Data> Results { get; set; }

        public Function3Response()
        {
        }

        public Function3Response(
            PaginationOptions paginationOptions,
            IEnumerable<Function3Data> function3DataList)
        {
            this.Page = paginationOptions.Page;
            this.PageSize = paginationOptions.PageSize;
            this.Results = function3DataList.Take(paginationOptions.PageSize);
            this.HasMoreResults = function3DataList.Count() > this.PageSize;
        }
    }
}
