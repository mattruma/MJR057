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
                IAsyncCollector<OrderDeliverData> orderDeliverDataCollector,
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
                await ordersCosmosContainer.ReadItemAsync<OrderDeliverData>(
                    orderIdParserResponse.Id,
                    new PartitionKey(orderIdParserResponse.LocationIdAndDate));

            var orderDeliverData =
                ordersCosmosItemResponse.Resource;

            var createMessageOptions =
                new CreateMessageOptions(
                    new PhoneNumber(orderDeliverData.CustomerPhoneNumber));

            if (orderDeliverRequest.HasDelivered)
            {
                orderDeliverData.DeliveredAt = DateTime.UtcNow;

                createMessageOptions.Body = Environment.GetEnvironmentVariable("ORDERDELIVER_TEMPLATE");

                createMessageOptions.Body = createMessageOptions.Body.Replace("{{CustomerName}}", orderDeliverData.CustomerName);
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{CustomerPhoneNumber}}", orderDeliverData.CustomerPhoneNumber);
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{DeliveredAtDate}}", orderDeliverData.DeliveredAt.Value.ToShortDateString());
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{DeliveredAtTime}}", orderDeliverData.DeliveredAt.Value.ToShortTimeString());
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{Id}}", orderDeliverData.Id);
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{OrderId}}", orderDeliverData.OrderId);

                await createMessageOptionsCollector.AddAsync(createMessageOptions);
            }
            else
            {
                orderDeliverData.DeliveredAt = null;
            }

            await orderDeliverDataCollector.AddAsync(orderDeliverData);

            var orderDeliverResponse =
                new OrderDeliverResponse(
                    orderDeliverData);

            return new OkObjectResult(orderDeliverResponse);
        }
    }
}
