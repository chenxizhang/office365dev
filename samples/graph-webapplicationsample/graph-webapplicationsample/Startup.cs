using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace graph_webapplicationsample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}