using ClassLibrary1.Data;
using ClassLibrary1.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace FunctionApp2
{
    public class OrderDeliver
    {
        private readonly CosmosClient _cosmosClient;
        private readonly OrderIdParser _orderIdParser;

        public OrderDeliver(
            CosmosClient cosmosClient,
            OrderIdParser orderIdParser)
        {
            _cosmosClient = cosmosClient;
            _orderIdParser = orderIdParser ?? throw new ArgumentNullException(nameof(orderIdParser));
        }

        [FunctionName(nameof(OrderDeliver))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "orders/{id}/deliver")] OrderDeliverRequest orderDeliverRequest,
            string id,
            [CosmosDB(
                databaseName: "%COSMOSDB_DATABASEID%",
                collectionName: "orders",
                ConnectionStringSetting = "COSMOSDB_CONNECTIONSTRING")]
                IAsyncCollector<OrderData> orderDataCollector,
            [TwilioSms(
                AccountSidSetting = "TWILIO_ACCOUNTSIDSETTING",
                AuthTokenSetting = "TWILIO_AUTHTOKENSETTING",
                From = "%TWILIO_FROM%")] IAsyncCollector<CreateMessageOptions> createMessageOptionsCollector,
            ILogger logger)
        {
            logger.LogInformation($"{nameof(OrderDeliver)} function processed a request.");

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

            var createMessageOptions =
                new CreateMessageOptions(
                    new PhoneNumber(orderData.CustomerPhoneNumber));

            if (orderDeliverRequest.HasDelivered)
            {
                orderData.DeliveredAt = DateTime.UtcNow;

                createMessageOptions.Body = Environment.GetEnvironmentVariable("ORDERDELIVER_TEMPLATE");

                createMessageOptions.Body = createMessageOptions.Body.Replace("{{CustomerName}}", orderData.CustomerName);
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{DeliveredAtDate}}", orderData.DeliveredAt.Value.ToShortDateString());
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{DeliveredAtTime}}", orderData.DeliveredAt.Value.ToShortTimeString());
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{Id}}", orderData.Id);

                await createMessageOptionsCollector.AddAsync(createMessageOptions);
            }
            else
            {
                orderData.DeliveredAt = null;
            }

            await orderDataCollector.AddAsync(orderData);

            var orderDeliverResponse =
                new OrderDeliverResponse(
                    orderData);

            return new OkObjectResult(orderDeliverResponse);
        }
    }
}
