using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Identity.Client;
using System.Configuration;

[assembly: OwinStartup(typeof(graph_webapplicationsample_aspnetmvc.Startup))]

namespace graph_webapplicationsample_aspnetmvc
{

    public class SessionToken : TokenCache
    {

    }

    public class Startup
    {
        private static string appId = ConfigurationManager.AppSettings["ida:AppId"];
        private static string appSecret = ConfigurationManager.AppSettings["ida:AppSecret"];
        private static string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        private static string graphScopes = ConfigurationManager.AppSettings["ida:GraphScopes"];
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = appId,
                    Authority = "https://login.microsoftonline.com/common/v2.0",
                    PostLogoutRedirectUri =redirectUri,
                    RedirectUri =redirectUri,
                    Scope = "openid email profile offline_access "+ graphScopes,
                    TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer=false
                    },
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        AuthorizationCodeReceived = async (context) =>
                        {
                            var code = context.Code;
                            string signedInUserID = context.AuthenticationTicket.Identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
                            
                        }
                    }
                });
        }
    }
}
