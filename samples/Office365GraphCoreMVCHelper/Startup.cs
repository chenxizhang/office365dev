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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Office365GraphCoreMVCHelper
{
    public class Startup
    {

        static IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                            .SetBasePath(env.ContentRootPath)
                            .AddJsonFile("appsettings.json")
                            .Build();
            // ClientId = Configuration["Office365ApplicationInfo:ClientId"];
            // Authority = Configuration["Office365ApplicationInfo:Authority"];
            // ClientSecret = Configuration["Office365ApplicationInfo:ClientSecret"];
            // GraphResourceId = Configuration["Office365ApplicationInfo:GraphResourceId"];
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddOptions();
            services.Configure<AppSetting>(Configuration);

            services.AddSession();
            services.AddAuthentication(sharedoptions => sharedoptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);

            // Add framework services.
            services.AddMvc();
        }


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

            //ConfigureMiddleware(app,env,loggerFactory);

            app.UseStaticFiles();
            app.UseSession();
            app.UseCookieAuthentication();

            var options = app.ApplicationServices.GetRequiredService<IOptions<AppSetting>>();

            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {
                ClientId = options.Value.Office365ApplicationInfo.ClientId,
                Authority = options.Value.Office365ApplicationInfo.Authority,
                ClientSecret = options.Value.Office365ApplicationInfo.ClientSecret,
                ResponseType = OpenIdConnectResponseType.CodeIdToken,
                CallbackPath = "/signin-oidc",
                GetClaimsFromUserInfoEndpoint = true,
                Events = new OpenIdConnectEvents
                {
                    OnAuthorizationCodeReceived = async (context) =>
                    {
                        string userObjectId = (context.Ticket.Principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;
                        ClientCredential clientCred = new ClientCredential(options.Value.Office365ApplicationInfo.ClientId, options.Value.Office365ApplicationInfo.ClientSecret);
                        AuthenticationContext authContext = new AuthenticationContext(options.Value.Office365ApplicationInfo.Authority, new SampleSessionCache(userObjectId, context.HttpContext.Session));
                        AuthenticationResult authResult = await authContext.AcquireTokenByAuthorizationCodeAsync(
                            context.ProtocolMessage.Code, new Uri(context.Properties.Items[OpenIdConnectDefaults.RedirectUriForCodePropertiesKey]), clientCred, options.Value.Office365ApplicationInfo.GraphResourceId);
                    }
                }
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private Task OnAuthenticationFailed(FailureContext context)
        {
            context.HandleResponse();
            context.Response.Redirect("/Home/Error?message=" + context.Failure.Message);
            return Task.FromResult(0);
        }
    }
}
