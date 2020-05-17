using ClassLibrary1.Data;
using ClassLibrary1.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace FunctionApp2
{
    public class OrderReceive
    {
        private readonly OrderIdBuilder _orderIdBuilder;

        public OrderReceive(
            OrderIdBuilder orderIdBuilder)
        {
            _orderIdBuilder = orderIdBuilder;
        }

        [FunctionName(nameof(OrderReceive))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "orders")] OrderReceiveRequest orderReceiveRequest,
            [CosmosDB(
                databaseName: "%COSMOSDB_DATABASEID%",
                collectionName: "orders",
                ConnectionStringSetting = "COSMOSDB_CONNECTIONSTRING")] IAsyncCollector<OrderData> orderReceiveDataCollector,
            [TwilioSms(
                AccountSidSetting = "TWILIO_ACCOUNTSIDSETTING",
                AuthTokenSetting = "TWILIO_AUTHTOKENSETTING",
                From = "%TWILIO_FROM%")] ICollector<CreateMessageOptions> createMessageOptionsCollector,
            ILogger logger)
        {
            logger.LogInformation($"{nameof(OrderReceive)} function processed a request.");

            var orderIdBuilderResponse =
                _orderIdBuilder.Build(
                    orderReceiveRequest.OrderId,
                    orderReceiveRequest.LocationId,
                    orderReceiveRequest.Date);

            var orderData =
                new OrderData
                {
                    Id = orderIdBuilderResponse.Id,
                    Date = orderReceiveRequest.Date,
                    OrderId = orderReceiveRequest.OrderId,
                    CustomerName = orderReceiveRequest.CustomerName,
                    CustomerPhoneNumber = orderReceiveRequest.CustomerPhoneNumber.Sanitize("[^0-9]"),
                    LocationId = orderReceiveRequest.LocationId,
                    LocationIdAndDate = orderIdBuilderResponse.LocationIdAndDate,
                    ReadyAt = orderReceiveRequest.ReadyAt
                };

            await orderReceiveDataCollector.AddAsync(orderData);

            var createMessageOptions =
                new CreateMessageOptions(
                    new PhoneNumber(orderData.CustomerPhoneNumber))
                {
                    Body = Environment.GetEnvironmentVariable("ORDERRECEIVED_TEMPLATE")
                };

            createMessageOptions.Body = createMessageOptions.Body.Replace("{{CustomerName}}", orderData.CustomerName);
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{ReadyAtDate}}", orderData.ReadyAt.ToShortDateString());
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{ReadyAtTime}}", orderData.ReadyAt.ToShortTimeString());
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{Id}}", orderData.Id);

            createMessageOptionsCollector.Add(createMessageOptions);

            var orderReceiveResponse =
                new OrderReceiveResponse(
                    orderData);

            return new CreatedResult("", orderReceiveResponse);
        }
    }
}
