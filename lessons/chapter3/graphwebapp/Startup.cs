/*
作者：陈希章 Ares Chen
时间：2018年4月25日
说明：
    这是一个可以快速通过Microsoft Graph访问到Office 365资源的网站应用程序模板。
    目前该模板同时支持国际版和国内版。

关于此模板的使用以及问题反馈，请访问 https://github.com/chenxizhang/dotnetcore-office365dev-templates/tree/master/dotnetcore-graph-webapp
Office 365开发入门指南，请参考 https://github.com/chenxizhang/office365dev 
更多模板请参考 https://github.com/chenxizhang/dotnetcore-office365dev-templates 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Graph;
using System.Net.Http.Headers;
using System.Text;

namespace graphwebapp
{
    public class Startup
    {
        string clientid, authority, adminconsent, resource, secret;
        string ObjectIdentifierType = "http://schemas.microsoft.com/identity/claims/objectidentifier";
        
        public Startup(){
            #region parameters
            

            clientid = "5200f0d2-0ab3-4cc4-bb13-3506a04106e0";
            secret = "fthpNSD50~#ppuWHNE130-+";

            authority ="https://login.microsoftonline.com/common";
            resource="https://graph.microsoft.com";
            

            #endregion

        }



        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMemoryCache();

            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddOpenIdConnect(options =>
                {
                    options.Authority = authority;
                    options.Resource = resource;
                    options.ClientId = clientid;
                    options.ClientSecret = secret;
                    options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false
                    };
                    options.Events = new OpenIdConnectEvents
                    {
                        OnTicketReceived = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = async (context) =>
                        {
                            await context.Response.WriteAsync("Fail");
                        },
                        OnAuthorizationCodeReceived = async (context) =>
                        {
                            var code = context.ProtocolMessage.Code;

                            var memorycache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
                            var identifier = context.Principal.FindFirst(ObjectIdentifierType).Value;
                            var sessionTokencache = new SessionTokenCache(identifier, memorycache);

                            var ctx = new AuthenticationContext(authority, sessionTokencache.GetCacheInstance());
                            var result = await ctx.AcquireTokenByAuthorizationCodeAsync(code, new Uri("http://localhost:5000/signin-oidc"), new ClientCredential(clientid, secret));
                            context.HandleCodeRedemption(result.AccessToken, result.IdToken);

                        }
                    };
                })
                .AddCookie();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            Func<HttpContext, GraphServiceClient> getGraphServiceClient = (context) =>
            {
                var identifier = context.User.FindFirst(ObjectIdentifierType).Value;
                var memorycache = context.RequestServices.GetRequiredService<IMemoryCache>();
                var sessionTokencache = new SessionTokenCache(identifier, memorycache);
                var ctx = new AuthenticationContext(authority, sessionTokencache.GetCacheInstance());
                var result = ctx.AcquireTokenSilentAsync(resource, new ClientCredential(clientid, secret), new UserIdentifier(identifier, UserIdentifierType.UniqueId)).Result;

                var graphserviceClient = new GraphServiceClient(new DelegateAuthenticationProvider(async (request) =>
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                    await Task.FromResult(0);
                }))
                {
                    BaseUrl = $"{resource}/v1.0"
                };

                return graphserviceClient;

            };


            app.Map("/messages", (builder) =>
            {
                builder.Run(async (context) =>
                {
                    context.Response.ContentType = "text/html;charset=utf-8";

                    if (context.User.Identity.IsAuthenticated)
                    {
                        var client = getGraphServiceClient(context);
                        var messages = await client.Me.Messages.Request().GetAsync();
                        await context.Response.WriteAsync($"<h1>我的邮件</h1>{string.Join("<br />", messages.Select(x => x.Subject))}", Encoding.UTF8);
                    }
                    else
                    {
                        await AuthenticationHttpContextExtensions.ChallengeAsync(context, OpenIdConnectDefaults.AuthenticationScheme);
                    }
                });
            });
            app.Map("/files", (builder) =>
            {
                builder.Run(async (context) =>
                {
                    context.Response.ContentType = "text/html;charset=utf-8";

                    if (context.User.Identity.IsAuthenticated)
                    {
                        var client = getGraphServiceClient(context);
                        var files = await client.Me.Drive.Root.Children.Request().GetAsync();
                        await context.Response.WriteAsync($"<h1>我的文件</h1>{string.Join("<br />", files.Select(x => x.Name))}", Encoding.UTF8);
                    }
                    else
                    {
                        await AuthenticationHttpContextExtensions.ChallengeAsync(context, OpenIdConnectDefaults.AuthenticationScheme);
                    }
                });
            });
            app.Run(async (context) =>
            {

                context.Response.ContentType = "text/html;charset=utf-8";

                if (context.User.Identity.IsAuthenticated)
                {

                    var client = getGraphServiceClient(context);
                    var me = await client.Me.Request().GetAsync();
                    var sb = new StringBuilder();
                    sb.Append("<h2>欢迎使用Microsoft Graph</h2>");
                    sb.Append("<p>个人信息</p>");
                    sb.Append($"<p>姓名:{me.DisplayName}</p>");
                    sb.Append($"<p>邮箱:{me.UserPrincipalName}</p>");
                    sb.Append($"<p><a href='/messages'>我的邮件</p>");
                    sb.Append($"<p><a href='/files'>我的文件</p>");

                    await context.Response.WriteAsync(sb.ToString(), Encoding.UTF8);
                }
                else
                {
                    await AuthenticationHttpContextExtensions.ChallengeAsync(context, OpenIdConnectDefaults.AuthenticationScheme);
                }
            });
        }
    }
}
