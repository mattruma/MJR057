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
            ILogger logger)
        {
            logger.LogInformation($"{nameof(Function2)} function processed a request.");

            var createMessageOptions =
                new CreateMessageOptions(
                    new PhoneNumber(function2Data.CustomerPhoneNumber));

            if (function3Request.HasArrived)
            {
                function2Data.ArrivedAt = DateTime.UtcNow;
            }
            else
            {
                function2Data.ArrivedAt = null;
            }

            await function3DataCollector.AddAsync(function2Data);

            var function2Response =
                new Function2Response(
                    function2Data);

            return new OkObjectResult(function2Response);
        }
    }
}
