// Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. See full license at the root of the repo.

/* 
    This file configures auth for the add-in. 
*/

using Owin;
using System.IdentityModel.Tokens;
using System.Configuration;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Jwt;
using Office_Add_in_ASPNET_SSO_WebAPI.App_Start;

namespace Office_Add_in_ASPNET_SSO_WebAPI
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            var tvps = new TokenValidationParameters
            {
                // Set the strings to validate against. (Scopes, which should be 
                // simply "access_as_user" in this sample, is validated inside the controller.)
                ValidAudience = ConfigurationManager.AppSettings["ida:Audience"],
                ValidIssuer = ConfigurationManager.AppSettings["ida:Issuer"],

                // Save the raw token recieved from the Office host, so it can be 
                // used in the "on behalf of" flow.
                SaveSigninToken = true
            };

            // The more familiar UseWindowsAzureActiveDirectoryBearerAuthentication does not work
            // with the Azure AD V2 endpoint, so use UseOAuthBearerAuthentication instead.
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
                AccessTokenFormat = new JwtFormat(tvps,

                    // Specify the discovery endpoint, also called the "metadata address".
                    new OpenIdConnectCachingSecurityTokenProvider("https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration"))
            });
        }
    }
}