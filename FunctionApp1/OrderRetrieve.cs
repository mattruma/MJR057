using ClassLibrary1.Data;
using ClassLibrary1.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public class OrderRetrieve
    {
        private readonly CosmosClient _cosmosClient;
        private readonly OrderIdParser _orderIdParser;

        public OrderRetrieve(
            CosmosClient cosmosClient,
            OrderIdParser orderIdParser)
        {
            _cosmosClient = cosmosClient;
            _orderIdParser = orderIdParser;
        }

        [FunctionName(nameof(OrderRetrieve))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "orders/{id}")] HttpRequest httpRequest,
            string id,
            ILogger logger)
        {
            logger.LogInformation($"{nameof(OrderRetrieve)} function processed a request.");

            var orderIdParserResponse =
                _orderIdParser.Parse(id);

            var ordersCosmosContainer =
                _cosmosClient.GetContainer(
                    Environment.GetEnvironmentVariable("COSMOSDB_DATABASEID"),
                    "orders");

            var ordersCosmosItemResponse =
                await ordersCosmosContainer.ReadItemAsync<OrderData>(
                    orderIdParserResponse.Id,
                    new PartitionKey(orderIdParserResponse.LocationIdAndDate));

            var orderData =
                ordersCosmosItemResponse.Resource;

            var orderRetrieveResponse =
                new OrderRetrieveResponse(
                    orderData);

            return new OkObjectResult(orderRetrieveResponse);
        }
    }
}
