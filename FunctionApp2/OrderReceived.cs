using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace FunctionApp2
{
    public static class OrderReceived
    {
        [FunctionName(nameof(OrderReceived))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "orders")] OrderReceivedRequest orderReceivedRequest,
            [CosmosDB(
                databaseName: "%COSMOSDB_DATABASEID%",
                collectionName: "orders",
                ConnectionStringSetting = "COSMOSDB_CONNECTIONSTRING")] IAsyncCollector<OrderReceivedData> orderReceivedDataCollector,
            [TwilioSms(
                AccountSidSetting = "TWILIO_ACCOUNTSIDSETTING",
                AuthTokenSetting = "TWILIO_AUTHTOKENSETTING",
                From = "%TWILIO_FROM%")] ICollector<CreateMessageOptions> createMessageOptionsCollector,
            ILogger logger)
        {
            logger.LogInformation($"{nameof(OrderReceived)} function processed a request.");

            Regex regex;

            regex = new Regex("[^0-9]");

            orderReceivedRequest.CustomerPhoneNumber = regex.Replace(orderReceivedRequest.CustomerPhoneNumber, "");

            regex = new Regex("[^a-zA-Z0-9]");

            orderReceivedRequest.OrderId = regex.Replace(orderReceivedRequest.OrderId, "").ToUpper();

            var orderReceivedData =
                new OrderReceivedData
                {
                    Id = $"{orderReceivedRequest.LocationId}-{orderReceivedRequest.Date:yyyyMMdd}-{orderReceivedRequest.OrderId}",
                    Date = orderReceivedRequest.Date,
                    OrderId = orderReceivedRequest.OrderId,
                    CustomerName = orderReceivedRequest.CustomerName,
                    CustomerPhoneNumber = orderReceivedRequest.CustomerPhoneNumber,
                    LocationId = orderReceivedRequest.LocationId,
                    LocationIdAndDate = $"{orderReceivedRequest.LocationId}-{orderReceivedRequest.Date:yyyyMMdd}",
                    ReadyAt = orderReceivedRequest.ReadyAt
                };

            await orderReceivedDataCollector.AddAsync(orderReceivedData);

            var createMessageOptions =
                new CreateMessageOptions(
                    new PhoneNumber(orderReceivedData.CustomerPhoneNumber))
                {
                    Body = Environment.GetEnvironmentVariable("ORDERRECEIVED_TEMPLATE")
                };

            createMessageOptions.Body = createMessageOptions.Body.Replace("{{CustomerName}}", orderReceivedData.CustomerName);
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{CustomerPhoneNumber}}", orderReceivedData.CustomerPhoneNumber);
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{ReadyAtDate}}", orderReceivedData.ReadyAt.ToShortDateString());
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{ReadyAtTime}}", orderReceivedData.ReadyAt.ToShortTimeString());
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{Id}}", orderReceivedData.Id);
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{OrderId}}", orderReceivedData.OrderId);
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{LocationId}}", orderReceivedData.LocationId);

            createMessageOptionsCollector.Add(createMessageOptions);

            var orderReceivedResponse =
                new OrderReceivedResponse(
                    orderReceivedData);

            return new CreatedResult("", orderReceivedResponse);
        }
    }
}
