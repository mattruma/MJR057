using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace FunctionApp1
{
    public static class Function3
    {
        [FunctionName(nameof(Function3))]
        public static void Run(
            [CosmosDBTrigger(
                databaseName: "%COSMOSDB_DATABASEID%",
                collectionName: "orders",
                ConnectionStringSetting = "COSMOSDB_CONNECTIONSTRING",
                StartFromBeginning = true,
                CreateLeaseCollectionIfNotExists = true,
                LeaseCollectionName = "leases")]IReadOnlyList<Document> documentList,
            [CosmosDB(
                databaseName: "%COSMOSDB_DATABASEID%",
                collectionName: "customers",
                ConnectionStringSetting = "COSMOSDB_CONNECTIONSTRING")] ICollector<Function3Data> function3DataCollector,
            [TwilioSms(
                AccountSidSetting = "TWILIO_ACCOUNTSIDSETTING",
                AuthTokenSetting = "TWILIO_AUTHTOKENSETTING",
                From = "%TWILIO_FROM%")] ICollector<CreateMessageOptions> createMessageOptionsCollector,
            ILogger logger)
        {
            logger.LogInformation($"{nameof(Function3)} function processed a request.");

            foreach (var document in documentList)
            {
                var function3Request =
                     new Function3Request(document);

                var function3Data =
                    new Function3Data(function3Request);

                function3DataCollector.Add(
                    function3Data);

                var createMessageOptions =
                    new CreateMessageOptions(
                        new PhoneNumber(function3Data.CustomerPhoneNumber))
                    {
                        Body = Environment.GetEnvironmentVariable("FUNCTION3_TEMPLATE")
                    };

                createMessageOptions.Body = createMessageOptions.Body.Replace("{{CustomerName}}", function3Data.CustomerName);
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{CustomerPhoneNumber}}", function3Data.CustomerPhoneNumber);
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{ReadyAtDate}}", function3Data.ReadyAt.ToShortDateString());
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{ReadyAtTime}}", function3Data.ReadyAt.ToShortTimeString());
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{Uri}}", Environment.GetEnvironmentVariable("FUNCTION3_URI"));
                createMessageOptions.Body = createMessageOptions.Body.Replace("{{OrderId}}", function3Data.OrderId);

                createMessageOptionsCollector.Add(createMessageOptions);
            }
        }
    }
}
