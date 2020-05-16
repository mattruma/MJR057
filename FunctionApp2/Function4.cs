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
    public class Function4
    {
        private readonly CosmosClient _cosmosClient;

        public Function4(
            CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        [FunctionName(nameof(Function4))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "locations")] HttpRequest httpRequest,
            ILogger logger)
        {
            logger.LogInformation($"{nameof(Function4)} function processed a request.");

            var paginationOptions =
                new PaginationOptions(
                    httpRequest);

            var cosmosContainer =
                _cosmosClient.GetContainer(
                    Environment.GetEnvironmentVariable("COSMOSDB_DATABASEID"),
                    "locations");

            var itemQueryable = cosmosContainer
                .GetItemLinqQueryable<Function4Data>();

            IQueryable<Function4Data> query = itemQueryable
                .OrderBy(x => x.Name);

            query = query
                .Skip((paginationOptions.Page - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize + 1);

            var feedIterator =
                query.ToFeedIterator();

            var function4DataList =
                new List<Function4Data>();

            while (feedIterator.HasMoreResults)
            {
                var feedResponse =
                    await feedIterator.ReadNextAsync();

                function4DataList.AddRange(
                    feedResponse.Resource);
            }

            var function4Response =
                new Function4Response(
                    paginationOptions,
                    function4DataList);

            return new OkObjectResult(function4Response);
        }
    }
}
