using FunctionApp2.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionApp2
{
    public class Function3
    {
        private readonly CosmosClient _cosmosClient;

        public Function3(
            CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        [FunctionName(nameof(Function3))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "locations/{locationId}/orders/{date}")] HttpRequest httpRequest,
            string locationId,
            string date,
            ILogger logger)
        {
            logger.LogInformation($"{nameof(Function3)} function processed a request.");

            var paginationOptions =
                new PaginationOptions(
                    httpRequest);

            var cosmosContainer =
                _cosmosClient.GetContainer(
                    Environment.GetEnvironmentVariable("COSMOSDB_DATABASEID"),
                    "orders");

            var itemQueryable = cosmosContainer
                .GetItemLinqQueryable<Function3Data>();

            IQueryable<Function3Data> query = itemQueryable
                .Where(x => x.OrderId == $"{locationId}{Convert.ToDateTime(date):yyyyMMdd}")
                .OrderByDescending(x => x.CreatedOn);

            query = query
                .Skip((paginationOptions.Page - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize + 1);

            var feedIterator =
                query.ToFeedIterator();

            var function3DataList =
                new List<Function3Data>();

            while (feedIterator.HasMoreResults)
            {
                var feedResponse =
                    await feedIterator.ReadNextAsync();

                function3DataList.AddRange(
                    feedResponse.Resource);
            }

            var function3Response =
                new Function3Response(
                    paginationOptions,
                    function3DataList);

            return new OkObjectResult(function3Response);
        }
    }
}
