using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName(nameof(Function1))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "customers/{customerPhoneNumber}/orders/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "%COSMOSDB_DATABASEID%",
                collectionName: "customers",
                ConnectionStringSetting = "COSMOSDB_CONNECTIONSTRING",
                Id = "{id}",
                PartitionKey = "{customerPhoneNumber}")] Function1Data function1Data,
            ILogger logger)
        {
            logger.LogInformation($"{nameof(Function1)} function processed a request.");

            if (function1Data == null)
            {
                return new NotFoundResult();
            }

            var function1Response =
                new Function1Response(
                    function1Data);

            return new OkObjectResult(function1Response);
        }
    }
}
