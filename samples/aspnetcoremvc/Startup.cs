using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Caching;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Session;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace aspntecoremvc
{
    public class Startup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddAuthentication(sharedoptions => sharedoptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
           
            // Add framework services.
            services.AddMvc();
        }

        private readonly string ClientId="e91ef175-e38d-4feb-b1ed-f243a6a81b93";
        private readonly string Authority=String.Format("https://login.microsoftonline.com/{0}","office365devlabs.onmicrosoft.com");
        private readonly string ClientSecret="2F5jdoGGNn59oxeDLE9fXx5tD86uvzIji74dmLaj3YI=";
        private readonly string GraphResourceId="https://graph.microsoft.com";
        private readonly string CallbackPath ="/signin-oidc";

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseCookieAuthentication();
            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions{
                ClientId = ClientId,
                Authority = Authority,
                ClientSecret = ClientSecret,
                ResponseType = OpenIdConnectResponseType.CodeIdToken,
                CallbackPath = CallbackPath,
                GetClaimsFromUserInfoEndpoint =true,
                Events = new OpenIdConnectEvents{
                    OnAuthorizationCodeReceived = OnAuthorizationCodeReceived
                }
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        private async Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
        {
            // Acquire a Token for the Graph API and cache it using ADAL.  In the TodoListController, we'll use the cache to acquire a token to the Todo List API
            string userObjectId = (context.Ticket.Principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;
            ClientCredential clientCred = new ClientCredential(ClientId, ClientSecret);
            AuthenticationContext authContext = new AuthenticationContext(Authority, new SampleSessionCache(userObjectId, context.HttpContext.Session));
            AuthenticationResult authResult = await authContext.AcquireTokenByAuthorizationCodeAsync(
                context.ProtocolMessage.Code, new Uri(context.Properties.Items[OpenIdConnectDefaults.RedirectUriForCodePropertiesKey]), clientCred, GraphResourceId);

            
        }

        private Task OnAuthenticationFailed(FailureContext context)
        {
            context.HandleResponse();
            context.Response.Redirect("/Home/Error?message=" + context.Failure.Message);
            return Task.FromResult(0);
        }
    }
}
