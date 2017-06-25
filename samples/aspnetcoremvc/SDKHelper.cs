using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Graph;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;


namespace aspntecoremvc{
    public static class SDKHelper{
        

        public static async Task<GraphServiceClient> GetAuthenticatedClient(this ControllerBase controller){

            var Authority = String.Format("https://login.microsoftonline.com/{0}","office365devlabs.onmicrosoft.com");
            var ClientId = "e91ef175-e38d-4feb-b1ed-f243a6a81b93";
            var ClientSecret = "2F5jdoGGNn59oxeDLE9fXx5tD86uvzIji74dmLaj3YI=";
            var GraphResourceId = "https://graph.microsoft.com";


            string userObjectId = controller.HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
            ClientCredential clientCred = new ClientCredential(ClientId, ClientSecret);
            AuthenticationContext authContext = new AuthenticationContext(Authority, new SampleSessionCache(userObjectId, controller.HttpContext.Session));
            AuthenticationResult result = await authContext.AcquireTokenSilentAsync(GraphResourceId,ClientId);

            GraphServiceClient client = new GraphServiceClient(new DelegateAuthenticationProvider(async request=>{
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);
                await Task.FromResult(0);
            }));

            return client;

        }

    }

}