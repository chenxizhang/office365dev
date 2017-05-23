/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/
#if MSGRAPH

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft_Graph_REST_ASPNET_Connect.TokenStorage;
using Resources;

namespace Microsoft_Graph_REST_ASPNET_Connect.Helpers
{
    public sealed class SampleAuthProvider : IAuthProvider
    {
        private SampleAuthProvider() { } 

        public static SampleAuthProvider Instance { get;  } = new SampleAuthProvider();

        // Get an access token. First tries to get the token from the token cache.
        public async Task<string> GetUserAccessTokenAsync()
        {
            string signedInUserID = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            HttpContextBase httpContextBase = HttpContext.Current.GetOwinContext().Environment["System.Web.HttpContextBase"] as HttpContextBase;

            SessionTokenCache tokenCache = new SessionTokenCache(signedInUserID, httpContextBase);
            //var cachedItems = tokenCache.ReadItems(); // see what's in the cache

            AuthenticationContext authContext = new AuthenticationContext(SettingsHelper.Authority, tokenCache);
            ClientCredential clientCredential = new ClientCredential(SettingsHelper.ClientId, SettingsHelper.ClientSecret);

            string userObjectId = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            UserIdentifier userId = new UserIdentifier(userObjectId, UserIdentifierType.UniqueId);

            try
            {
                AuthenticationResult result = await authContext.AcquireTokenSilentAsync(SettingsHelper.GraphResourceId, clientCredential, userId);
                return result.AccessToken;
            }
            // Unable to retrieve the access token silently.
            catch (AdalException ex)
            {
                HttpContext.Current.Request.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties() { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);

                throw new Exception(Resource.Error_AuthChallengeNeeded + $" {ex.Message}");
            }
        }
    }
}

#endif