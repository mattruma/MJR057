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
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "customers/{customerPhoneNumber}/orders/{id}")] Function2Request function3Request,
            [CosmosDB(
                databaseName: "%COSMOSDB_DATABASEID%",
                collectionName: "customers",
                ConnectionStringSetting = "COSMOSDB_CONNECTIONSTRING",
                Id = "{id}",
                PartitionKey = "{customerPhoneNumber}")] Function2Data function2Data,
            [CosmosDB(
                databaseName: "%COSMOSDB_DATABASEID%",
                collectionName: "customers",
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

                createMessageOptions.Body = Environment.GetEnvironmentVariable("FUNCTION2_TEMPLATE_1");
            }
            else
            {
                function2Data.ArrivedAt = null;

                createMessageOptions.Body = Environment.GetEnvironmentVariable("FUNCTION2_TEMPLATE_2");
            }

            createMessageOptions.Body = createMessageOptions.Body.Replace("{{CustomerName}}", function2Data.CustomerName);
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{CustomerPhoneNumber}}", function2Data.CustomerPhoneNumber);
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{Uri}}", Environment.GetEnvironmentVariable("FUNCTION2_URI"));
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{Id}}", function2Data.Id);
            createMessageOptions.Body = createMessageOptions.Body.Replace("{{OrderId}}", function2Data.OrderId);

            await function3DataCollector.AddAsync(function2Data);

            await createMessageOptionsCollector.AddAsync(createMessageOptions);

            var function2Response =
                new Function2Response(
                    function2Data);

            return new OkObjectResult(function2Response);
        }
    }
}
