using Microsoft.Graph;
using System.Net.Http.Headers;

namespace MicrosoftGraphSample_ASPNETMVC.Controllers
{
    public class SDKHelper
    {

        // Get an authenticated Microsoft Graph Service client.
        public static GraphServiceClient GetAuthenticatedClient()
        {
            GraphServiceClient graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        string accessToken = await SampleAuthProvider.Instance.GetUserAccessTokenAsync();

                        // Append the access token to the request.
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                    }));
            return graphClient;
        }

    }

}