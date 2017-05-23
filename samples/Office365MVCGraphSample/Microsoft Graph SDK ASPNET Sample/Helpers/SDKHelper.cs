/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

#if MSGRAPH
using System.Net.Http.Headers;
using Microsoft.Graph;
using Microsoft_Graph_REST_ASPNET_Connect;
using Microsoft_Graph_REST_ASPNET_Connect.Helpers;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Helpers
{
    public class SDKHelper
    {   
        private static GraphServiceClient graphClient = null;

        // Get an authenticated Microsoft Graph Service client.
        public static GraphServiceClient GetAuthenticatedClient()
        {
            graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        string accessToken = await SampleAuthProvider.Instance.GetUserAccessTokenAsync();

                        // Append the access token to the request.
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                        // Add this header hto identify the sample in the Microsoft Graph service.
                        // requestMessage.Headers.Add("SampleID", "AppName");
                    }));
            return graphClient;
        }

        public static void SignOutClient()
        {
            graphClient = null;
        }
    }
}

#endif