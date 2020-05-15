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
    public static class Function2
    {
        [FunctionName(nameof(Function2))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "orders/{id}")] Function2Request function4Request,
            [CosmosDB(
                databaseName: "%COSMOSDB_DATABASEID%",
                collectionName: "orders",
                ConnectionStringSetting = "COSMOSDB_CONNECTIONSTRING",
                Id = "{id}",
                PartitionKey = "{id}")] Function2Data function2Data,
            [CosmosDB(
                databaseName: "%COSMOSDB_DATABASEID%",
                collectionName: "orders",
                ConnectionStringSetting = "COSMOSDB_CONNECTIONSTRING")]
                IAsyncCollector<Function2Data> function4DataCollector,
            [TwilioSms(
                AccountSidSetting = "TWILIO_ACCOUNTSIDSETTING",
                AuthTokenSetting = "TWILIO_AUTHTOKENSETTING",
                From = "%TWILIO_FROM%")] IAsyncCollector<CreateMessageOptions> createMessageOptionsCollector,
            ILogger logger)
        {
            logger.LogInformation($"{nameof(Function2)} function processed a request.");

            var createMessageOptions =
                new CreateMessageOptions(
                        new PhoneNumber(function2Data.CustomerPhoneNumber));

            if (function4Request.HasDelivered)
            {
                function2Data.DeliveredAt = DateTime.UtcNow;

                createMessageOptions.Body = $"Hi {function2Data.CustomerName}, thanks again for your order! Hope it was worth the drive!";
            }
            else
            {
                function2Data.DeliveredAt = null;
            }

            await function4DataCollector.AddAsync(function2Data);

            if (function4Request.HasDelivered)
            {
                await createMessageOptionsCollector.AddAsync(createMessageOptions);
            }

            return new OkResult();
        }
    }
}
