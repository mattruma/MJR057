using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            ILogger logger)
        {
            logger.LogInformation($"{nameof(Function1)} function processed a request.");

            var regex = new Regex("[^0-9]");

            function1Request.CustomerPhoneNumber = regex.Replace(function1Request.CustomerPhoneNumber, "");

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

            var function1Response =
                new Function1Response(
                    function1Data);

            return new CreatedResult("", function1Response);
        }
    }
}
