using ClassLibrary1.Domain;
using ClassLibrary1.Helpers;
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
    public class LocationList
    {
        private readonly CosmosClient _cosmosClient;

        public LocationList(
            CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        [FunctionName(nameof(LocationList))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "locations")] HttpRequest httpRequest,
            ILogger logger)
        {
            logger.LogInformation($"{nameof(LocationList)} function processed a request.");

            var paginationOptions =
                new PaginationOptions(
                    httpRequest);

            var locationCosmosContainer =
                _cosmosClient.GetContainer(
                    Environment.GetEnvironmentVariable("COSMOSDB_DATABASEID"),
                    "locations");

            IQueryable<LocationData> locationQuery =
                locationCosmosContainer
                    .GetItemLinqQueryable<LocationData>()
                    .OrderBy(x => x.Name);

            locationQuery = locationQuery
                .Skip((paginationOptions.Page - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize + 1);

            var locationFeedIterator =
                locationQuery.ToFeedIterator();

            var locationDataList =
                new List<LocationData>();

            while (locationFeedIterator.HasMoreResults)
            {
                var locationFeedResponse =
                    await locationFeedIterator.ReadNextAsync();

                locationDataList.AddRange(
                    locationFeedResponse.Resource);
            }

            var locationListResponse =
                new LocationListResponse(
                    paginationOptions,
                    locationDataList.Select(x => new Location(x)));

            return new OkObjectResult(locationListResponse);
        }
    }
}
