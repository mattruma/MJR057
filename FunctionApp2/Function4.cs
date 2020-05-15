using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FunctionApp2
{
    public static class Function4
    {
        [FunctionName(nameof(Function4))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "locations")] HttpRequest httpRequest,
            [CosmosDB(
                databaseName: "%COSMOSDB_DATABASEID%",
                collectionName: "locations",
                ConnectionStringSetting = "COSMOSDB_CONNECTIONSTRING",
                SqlQuery = "SELECT * FROM c ORDER BY c.name")]
                IEnumerable<Function4Data> function4DataList,
            ILogger logger)
        {
            logger.LogInformation($"{nameof(Function4)} function processed a request.");

            return new OkObjectResult(function4DataList);
        }
    }
}
