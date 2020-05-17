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
                IAsyncCollector<CustomerArriveData> customerArriveDataCollector,
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
                await ordersCosmosContainer.ReadItemAsync<CustomerArriveData>(
                    orderIdParserResponse.Id,
                    new PartitionKey(orderIdParserResponse.LocationIdAndDate));

            var customerArriveData =
                ordersCosmosItemResponse.Resource;

            var createMessageOptions =
                new CreateMessageOptions(
                    new PhoneNumber(customerArriveData.CustomerPhoneNumber));

            if (customerArriveRequest.HasArrived)
            {
                customerArriveData.ArrivedAt = DateTime.UtcNow;

                createMessageOptions.Body = Environment.GetEnvironmentVariable("CUSTOMERARRIVE_TEMPLATE_1");
            }
            else
            {
                customerArriveData.ArrivedAt = null;

                createMessageOptions.Body = Environment.GetEnvironmentVariable("CUSTOMERARRIVE_TEMPLATE_2");
            }

            await customerArriveDataCollector.AddAsync(customerArriveData);

            createMessageOptions.Body = createMessageOptions.Body.Replace("{{CustomerName}}", customerArriveData.CustomerName);
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{CustomerPhoneNumber}}", customerArriveData.CustomerPhoneNumber);
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{ReadyAtDate}}", customerArriveData.ReadyAt.ToShortDateString());
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{ReadyAtTime}}", customerArriveData.ReadyAt.ToShortTimeString());
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{Id}}", customerArriveData.Id);
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{OrderId}}", customerArriveData.OrderId);

            await createMessageOptionsCollector.AddAsync(createMessageOptions);

            var customerArriveResponse =
                new CustomerArriveResponse(
                    customerArriveData);

            return new OkObjectResult(customerArriveResponse);
        }
    }
}
