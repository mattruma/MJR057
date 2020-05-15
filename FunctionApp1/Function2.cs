using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace FunctionApp1
{
    public static class Function2
    {
        [FunctionName(nameof(Function2))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "orders/{id}")] Function2Request function3Request,
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
                IAsyncCollector<Function2Data> function3DataCollector,
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

            if (function3Request.HasArrived)
            {
                function2Data.ArrivedAt = DateTime.UtcNow;

                createMessageOptions.Body = $"Hi {function2Data.CustomerName}, looks like you arrived safely! One of our team members will be right out with your order. If you are not here, you can update your status https://mylink.com!";
            }
            else
            {
                function2Data.ArrivedAt = null;

                createMessageOptions.Body = $"Hi {function2Data.CustomerName}, drive safely! Let us know when you are here at https://mylink.com!";
            }

            await function3DataCollector.AddAsync(function2Data);

            await createMessageOptionsCollector.AddAsync(createMessageOptions);

            var function2Response =
                new Function2Response
                {
                    OrderId = function2Data.OrderId,
                    CustomerName = function2Data.CustomerName,
                    ArrivedAt = function2Data.ArrivedAt
                };

            return new OkObjectResult(function2Response);
        }
    }
}
