using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using UserManagement.Models;

namespace UserManagement.Services
{
    public class GraphClient
    {
        private static GraphServiceClient graphClient;
        private static IConfiguration configuration;
        private static string clientId;
        private static string clientSecret;
        private static string tenantId;
        private static string aadInstance;
        private static string graphResource;
        private static string graphAPIEndpoint;
        private static string authority;
        static GraphClient()
        {
            configuration = new ConfigurationBuilder().SetBasePath(System.IO.Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                                        .AddEnvironmentVariables()
                                        .Build();
            ConfigureAzureAD();
        }

        private static void ConfigureAzureAD()
        {
            var azureOptions = new ADConfig();
            configuration.Bind("AzureAD", azureOptions);
            clientId = azureOptions.ClientId;
            clientSecret = azureOptions.ClientSecret;
            tenantId = azureOptions.TenantId;
            aadInstance = azureOptions.Instance;
            graphResource = azureOptions.GraphResource;
            graphAPIEndpoint = $"{azureOptions.GraphResource}{azureOptions.GraphResourceEndPoint}";
            authority = $"{aadInstance}{tenantId}";
        }

        public static async Task<GraphServiceClient> GetServiceClient()
        {   
            //Getting Graphclient API Endpoints
            var delegateAuthProvider = await AuthProvider();
            // Initializing the GraphServiceClient with Auth
            graphClient = new GraphServiceClient(graphAPIEndpoint, delegateAuthProvider);
            return graphClient;
        }
        private static async Task<DelegateAuthenticationProvider> AuthProvider()
        {
            AuthenticationContext authenticationContext = new AuthenticationContext(authority);
            ClientCredential ClientCred = new ClientCredential(clientId, clientSecret);
            //Getting bearer token for Auth
            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenAsync(graphResource, ClientCred);
            var token = authenticationResult.AccessToken;
            var delegateAuthProvider = new DelegateAuthenticationProvider((requestMessage) => {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token.ToString());
                return Task.FromResult(0);
            });
            return delegateAuthProvider;
        }
    }
}
