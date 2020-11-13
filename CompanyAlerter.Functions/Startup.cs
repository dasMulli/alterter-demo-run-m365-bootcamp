using System;
using System.Collections.Generic;
using System.Text;
using CompanyAlerter.Functions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;

[assembly: FunctionsStartup(typeof(Startup))]

namespace CompanyAlerter.Functions
{
    public class SecurityConfig
    {
        public string TenantId { get; set; }

        public string Audience { get; set; }

        public string AppId { get; set; }

        public string ClientSecret { get; set; }
    }

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var securityConfig = new SecurityConfig
            {
                TenantId = "ef660bca-495a-4ed8-88d3-38ce8741a9fa",
                AppId = "cafcc835-bfd8-4aa5-9c34-c56ec57d590c",
                Audience = "api://alerter-demo",
                ClientSecret = Environment.GetEnvironmentVariable("OAUTH_CLIENT_SECRET")
            };
            builder.Services.AddSingleton(securityConfig);
            builder.Services.AddSingleton<AlerterSecurity>();

            var clientApplication = ConfidentialClientApplicationBuilder
                .Create(securityConfig.AppId)
                .WithClientSecret(securityConfig.ClientSecret)
                .WithTenantId(securityConfig.TenantId)
                .Build();

            var graphServiceClient = new GraphServiceClient(new ClientCredentialProvider(clientApplication));
            builder.Services.AddSingleton<IGraphServiceClient>(graphServiceClient);
        }
    }
}
