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
    public class OrderList
    {
        private readonly CosmosClient _cosmosClient;

        public OrderList(
            CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        [FunctionName(nameof(OrderList))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "locations/{locationId}/orders")] HttpRequest httpRequest,
            string locationId,
            ILogger logger)
        {
            logger.LogInformation($"{nameof(OrderList)} function processed a request.");

            var paginationOptions =
                new PaginationOptions(
                    httpRequest);

            var ordersCosmosContainer =
                _cosmosClient.GetContainer(
                    Environment.GetEnvironmentVariable("COSMOSDB_DATABASEID"),
                    "orders");

            IQueryable<OrderListData> ordersQuery =
                ordersCosmosContainer
                    .GetItemLinqQueryable<OrderListData>()
                    .Where(x => x.LocationId == locationId);

            if (DateTime.TryParse(httpRequest.Query["date"], out DateTime date))
            {
                ordersQuery = ordersQuery
                    .Where(x => x.LocationIdAndDate == $"{locationId}-{date:yyyyMMdd}");
            }

            ordersQuery = ordersQuery
                .OrderByDescending(x => x.Date)
                .Skip((paginationOptions.Page - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize + 1);

            var ordersFeedIterator =
                ordersQuery.ToFeedIterator();

            var orderListDataList =
                new List<OrderListData>();

            while (ordersFeedIterator.HasMoreResults)
            {
                var orderFeedResponse =
                    await ordersFeedIterator.ReadNextAsync();

                orderListDataList.AddRange(
                    orderFeedResponse.Resource);
            }

            var orderListResponse =
                new OrderListResponse(
                    paginationOptions,
                    orderListDataList);

            return new OkObjectResult(orderListResponse);
        }
    }
}
