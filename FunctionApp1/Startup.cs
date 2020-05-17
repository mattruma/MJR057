using ClassLibrary1.Helpers;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

[assembly: FunctionsStartup(typeof(FunctionApp1.Startup))]
namespace FunctionApp1
{
    [ExcludeFromCodeCoverage]
    public class Startup : FunctionsStartup
    {
        public override void Configure(
            IFunctionsHostBuilder builder)
        {
            var services =
                builder.Services;

            services.AddSingleton(options =>
            {
                return new CosmosClientBuilder(
                    Environment.GetEnvironmentVariable("COSMOSDB_CONNECTIONSTRING"))
                    .WithConnectionModeDirect()
                    .Build();
            });

            services.AddSingleton(
                new OrderIdParser());
        }
    }
}
