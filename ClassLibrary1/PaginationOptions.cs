using Microsoft.AspNetCore.Http;

namespace ClassLibrary1
{
    public class PaginationOptions
    {
        public int Page { get; internal set; }

        public int PageSize { get; internal set; }

        public PaginationOptions()
        {
            this.Page = 1;
            this.PageSize = 20;
        }

        public PaginationOptions(
            HttpRequest httpRequest)
        {
            this.Page = 1;
            this.PageSize = 20;

            if (int.TryParse(httpRequest.Query["page"], out int page))
            {
                this.Page = page;
            }

            if (int.TryParse(httpRequest.Query["pageSize"], out int pageSize))
            {
                this.PageSize = pageSize;
            }
        }
    }
}
