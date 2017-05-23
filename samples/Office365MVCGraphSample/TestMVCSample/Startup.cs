using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Office365GraphMVCHelper;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Web;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.IdentityModel.Claims;

[assembly: OwinStartup(typeof(TestMVCSample.Startup))]

namespace TestMVCSample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

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

                            Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext authContext = new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext(SettingsHelper.Authority, tokenCache);
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
        }
    }
}
