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

namespace FunctionApp1
{
    public class CustomerArrive
    {
        private readonly CosmosClient _cosmosClient;
        private readonly OrderIdParser _orderIdParser;

        public CustomerArrive(
            CosmosClient cosmosClient,
            OrderIdParser orderIdParser)
        {
            _cosmosClient = cosmosClient;
            _orderIdParser = orderIdParser;
        }

        [FunctionName(nameof(CustomerArrive))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "orders/{id}/arrive")] CustomerArriveRequest customerArriveRequest,
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
            logger.LogInformation($"{nameof(CustomerArrive)} function processed a request.");

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

            if (orderData.DeliveredAt != null)
            {
                return new BadRequestObjectResult("Order has already been delivered.");
            }

            var createMessageOptions =
                new CreateMessageOptions(
                    new PhoneNumber(orderData.CustomerPhoneNumber));

            if (customerArriveRequest.HasArrived)
            {
                orderData.ArrivedAt = DateTime.UtcNow;

                createMessageOptions.Body = Environment.GetEnvironmentVariable("CUSTOMERARRIVE_TEMPLATE_1");
            }
            else
            {
                orderData.ArrivedAt = null;

                createMessageOptions.Body = Environment.GetEnvironmentVariable("CUSTOMERARRIVE_TEMPLATE_2");
            }

            await orderDataCollector.AddAsync(orderData);

            createMessageOptions.Body = createMessageOptions.Body.Replace("{{CustomerName}}", orderData.CustomerName);
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{Id}}", orderData.Id);

            await createMessageOptionsCollector.AddAsync(createMessageOptions);

            var customerArriveResponse =
                new CustomerArriveResponse(
                    orderData);

            return new OkObjectResult(customerArriveResponse);
        }
    }
}
