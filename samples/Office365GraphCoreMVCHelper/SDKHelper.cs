using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;

namespace Office365GraphCoreMVCHelper
{
    public static class SDKHelper
    {
        public static async Task<GraphServiceClient> GetAuthenticatedClient(this ControllerBase controller,IOptions<AppSetting> options)
        {
            
            
            

            var Authority = options.Value.Office365ApplicationInfo.Authority;
            var ClientId = options.Value.Office365ApplicationInfo.ClientId;
            var ClientSecret = options.Value.Office365ApplicationInfo.ClientSecret;
            var GraphResourceId = options.Value.Office365ApplicationInfo.GraphResourceId;


            string userObjectId = controller.HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
            ClientCredential clientCred = new ClientCredential(ClientId, ClientSecret);
            AuthenticationContext authContext = new AuthenticationContext(Authority, new SampleSessionCache(userObjectId, controller.HttpContext.Session));
            AuthenticationResult result = await authContext.AcquireTokenSilentAsync(GraphResourceId, ClientId);

            GraphServiceClient client = new GraphServiceClient(new DelegateAuthenticationProvider(async request =>
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);
                await Task.FromResult(0);
            }));

            return client;

        }

    }
}
