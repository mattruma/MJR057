using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace FunctionApp2
{
    public static class Function5
    {
        [FunctionName(nameof(Function5))]
        public static void Run(
            [CosmosDBTrigger(
                databaseName: "%COSMOSDB_DATABASEID%",
                collectionName: "customers",
                ConnectionStringSetting = "COSMOSDB_CONNECTIONSTRING",
                CreateLeaseCollectionIfNotExists = true,
                LeaseCollectionName = "leases")]IReadOnlyList<Document> documentList,
            [CosmosDB(
                databaseName: "%COSMOSDB_DATABASEID%",
                collectionName: "orders",
                ConnectionStringSetting = "COSMOSDB_CONNECTIONSTRING")] ICollector<Function5Data> function5DataCollector,
            [TwilioSms(
                AccountSidSetting = "TWILIO_ACCOUNTSIDSETTING",
                AuthTokenSetting = "TWILIO_AUTHTOKENSETTING",
                From = "%TWILIO_FROM%")] ICollector<CreateMessageOptions> createMessageOptionsCollector,
            ILogger logger)
        {
            logger.LogInformation($"{nameof(Function5)} function processed a request.");

            foreach (var document in documentList)
            {
                var function5Request =
                     new Function5Request(document);

                var function5Data =
                    new Function5Data(function5Request);

                function5DataCollector.Add(
                    function5Data);

                var createMessageOptions =
                    new CreateMessageOptions(
                        new PhoneNumber(function5Data.CustomerPhoneNumber));

                if (function5Data.ArrivedAt.HasValue)
                {
                    function5Data.ArrivedAt = DateTime.UtcNow;

                    createMessageOptions.Body = Environment.GetEnvironmentVariable("FUNCTION2_TEMPLATE_1");
                }
                else
                {
                    function5Request.ArrivedAt = null;

                    createMessageOptions.Body = Environment.GetEnvironmentVariable("FUNCTION2_TEMPLATE_2");
                }

                createMessageOptions.Body = createMessageOptions.Body.Replace("{{CustomerName}}", function5Data.CustomerName);
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{CustomerPhoneNumber}}", function5Data.CustomerPhoneNumber);
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{ReadyAtDate}}", function5Data.ReadyAt.ToShortDateString());
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{ReadyAtTime}}", function5Data.ReadyAt.ToShortTimeString());
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{Uri}}", Environment.GetEnvironmentVariable("FUNCTION5_URI"));
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{Id}}", function5Data.Id);
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{OrderId}}", function5Data.OrderId);

                createMessageOptionsCollector.Add(createMessageOptions);
            }
        }
    }
}
