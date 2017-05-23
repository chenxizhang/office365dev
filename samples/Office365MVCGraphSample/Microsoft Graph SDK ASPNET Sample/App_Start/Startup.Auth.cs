/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

// Sample location: https://github.com/microsoftgraph/aspnet-connect-rest-sample/tree/last_v1_auth
using System;
using System.IdentityModel.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft_Graph_REST_ASPNET_Connect.Helpers;
using Owin;

#if MSGRAPH
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft_Graph_REST_ASPNET_Connect.TokenStorage;
#endif

namespace Microsoft_Graph_REST_ASPNET_Connect
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

#if MSGRAPH
            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = SettingsHelper.ClientId,
                    Authority = SettingsHelper.Authority,
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        AuthorizationCodeReceived = async (context) =>
                        {
                            var code = context.Code;
                            string signedInUserID = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                            HttpContextBase httpContext = context.OwinContext.Environment["System.Web.HttpContextBase"] as HttpContextBase;
                            TokenCache tokenCache = new SessionTokenCache(signedInUserID, httpContext);

                            AuthenticationContext authContext = new AuthenticationContext(SettingsHelper.Authority, tokenCache);
                            ClientCredential credential = new ClientCredential(SettingsHelper.ClientId, SettingsHelper.ClientSecret);
                            Uri redirectUri = new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path));
                            AuthenticationResult result = await authContext.AcquireTokenByAuthorizationCodeAsync(code, redirectUri, credential, SettingsHelper.GraphResourceId);
                            // var token = result.AccessToken;
                        },
                        AuthenticationFailed = (context) =>
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/Error?message=" + context.Exception.Message);
                            return Task.FromResult(0);
                        }
                }
        });
#endif
        }
    }
}