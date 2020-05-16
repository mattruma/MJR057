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
    public static class Function1
    {
        [FunctionName(nameof(Function1))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "orders")] Function1Request function1Request,
            [CosmosDB(
                databaseName: "%COSMOSDB_DATABASEID%",
                collectionName: "orders",
                ConnectionStringSetting = "COSMOSDB_CONNECTIONSTRING")] IAsyncCollector<Function1Data> function1DataCollector,
            [TwilioSms(
                AccountSidSetting = "TWILIO_ACCOUNTSIDSETTING",
                AuthTokenSetting = "TWILIO_AUTHTOKENSETTING",
                From = "%TWILIO_FROM%")] IAsyncCollector<CreateMessageOptions> createMessageOptionsCollector,
            ILogger logger)
        {
            logger.LogInformation($"{nameof(Function1)} function processed a request.");

            var function1Data =
                new Function1Data
                {
                    Id = function1Request.OrderId,
                    Date = function1Request.Date,
                    OrderId = $"{function1Request.LocationId}{function1Request.Date:yyyyMMdd}",
                    CustomerName = function1Request.CustomerName,
                    CustomerPhoneNumber = function1Request.CustomerPhoneNumber,
                    LocationId = function1Request.LocationId,
                    ReadyAt = function1Request.ReadyAt
                };

            await function1DataCollector.AddAsync(function1Data);

            var createMessageOptions =
                new CreateMessageOptions(
                    new PhoneNumber(function1Data.CustomerPhoneNumber));

            createMessageOptions.Body = Environment.GetEnvironmentVariable("FUNCTION1_TEMPLATE");

            createMessageOptions.Body = createMessageOptions.Body.Replace("{{CustomerName}}", function1Data.CustomerName);
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{CustomerPhoneNumber}}", function1Data.CustomerPhoneNumber);
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{ReadyAtDate}}", function1Data.ReadyAt.ToShortDateString());
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{ReadyAtTime}}", function1Data.ReadyAt.ToShortTimeString());
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{Id}}", function1Data.Id);
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{Uri}}", Environment.GetEnvironmentVariable("FUNCTION1_URI"));

            await createMessageOptionsCollector.AddAsync(createMessageOptions);

            return new OkResult();
        }
    }
}
