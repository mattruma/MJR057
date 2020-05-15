using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
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
                    OrderId = function1Request.OrderId,
                    CustomerName = function1Request.CustomerName,
                    CustomerPhoneNumber = function1Request.CustomerPhoneNumber,
                    LocationId = function1Request.LocationId,
                    ReadyAt = function1Request.ReadyAt
                };

            await function1DataCollector.AddAsync(function1Data);

            var createMessageOptions =
                new CreateMessageOptions(
                    new PhoneNumber(function1Request.CustomerPhoneNumber))
                {
                    Body = $"Hi {function1Request.CustomerName}, thanks for your order! Your order should be ready on {function1Request.ReadyAt.ToShortDateString()} at {function1Request.ReadyAt.ToShortTimeString()}. Please click the following link when you arrive https://mylink.com and let us know you are here!"
                };

            await createMessageOptionsCollector.AddAsync(createMessageOptions);

            return new OkResult();
        }
    }
}
