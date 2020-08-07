using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Services.Interfaces
{
    public interface IGraphClient
    {
        void ConfigureAzureAD();
        Task<GraphServiceClient> GetServiceClient();

        Task<DelegateAuthenticationProvider> AuthProvider();
    }
}
