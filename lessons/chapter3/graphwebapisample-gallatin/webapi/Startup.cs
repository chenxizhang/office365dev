using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography.X509Certificates;

namespace webapi
{

    public class Startup
    {
        public static string clientid, authority, adminconsent, resource, secret, issuerSigningKey, tenantid, validissuer, validAudience;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            #region parameters
            clientid = "ef15b8d4-2762-4516-99fa-e44b9a8746f5";
            secret = "2TV1LxjheL2gf19tCdGO4yna0x65pJyeLbovY0PqTCE=";
            tenantid = "12c0cdab-3c40-4e86-80b9-3e6f98d2d344";

            authority = "https://login.chinacloudapi.cn/common";
            resource = "https://microsoftgraph.chinacloudapi.cn";
            validissuer = "https://sts.chinacloudapi.cn/12c0cdab-3c40-4e86-80b9-3e6f98d2d344/";
            validAudience = "https://modtsp.partner.onmschina.cn/dotnet-obo-webapi";
            #endregion

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.Authority = authority;
                o.Audience = validAudience;
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = validissuer
                };
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc();
        }
    }

}
