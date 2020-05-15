using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FunctionApp2
{
    public static class Function3
    {
        [FunctionName(nameof(Function3))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "locations/{locationId}/orders")] HttpRequest httpRequest,
            [CosmosDB(
                databaseName: "%COSMOSDB_DATABASEID%",
                collectionName: "orders",
                ConnectionStringSetting = "COSMOSDB_CONNECTIONSTRING",
                SqlQuery = "SELECT * FROM c WHERE c.locationId = {locationId}")]
                IEnumerable<Function3Data> function3DataList,
            ILogger logger)
        {
            logger.LogInformation($"{nameof(Function3)} function processed a request.");

            return new OkObjectResult(function3DataList);
        }
    }
}
