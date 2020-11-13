using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CompanyAlerter
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.AdditionalScopesToConsent.Add("https://graph.microsoft.com/People.Read");
                options.ProviderOptions.AdditionalScopesToConsent.Add("https://graph.microsoft.com/User.Read");
                options.ProviderOptions.AdditionalScopesToConsent.Add("https://graph.microsoft.com/User.ReadBasic.All");
                options.ProviderOptions.AdditionalScopesToConsent.Add("https://graph.microsoft.com/Group.Read.All");
                options.ProviderOptions.AdditionalScopesToConsent.Add("https://graph.microsoft.com/Contacts.Read");
                options.ProviderOptions.AdditionalScopesToConsent.Add("https://graph.microsoft.com/Presence.Read");
                options.ProviderOptions.AdditionalScopesToConsent.Add("https://graph.microsoft.com/Presence.Read.All");
            });

            await builder.Build().RunAsync();
        }
    }
}