/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/


using System.Net.Http.Headers;
using Microsoft.Graph;


namespace Office365GraphMVCHelper
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

            graphClient.BaseUrl = SettingsHelper.GraphResourceId;

            return graphClient;
        }

        public static void SignOutClient()
        {
            graphClient = null;
        }
    }
}
