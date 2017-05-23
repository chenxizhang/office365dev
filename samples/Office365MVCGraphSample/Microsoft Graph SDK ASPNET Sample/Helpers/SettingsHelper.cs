using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Microsoft_Graph_REST_ASPNET_Connect.Helpers
{
    public class SettingsHelper
    {
        public static string GraphResourceId { get; } = "https://graph.microsoft.com";

        public static string AADInstance { get; } = ConfigurationManager.AppSettings["ida:AADInstance"];
        public static string ClientId { get;  } = ConfigurationManager.AppSettings["ida:ClientId"];
        public static string ClientSecret { get;  } = ConfigurationManager.AppSettings["ida:ClientSecret"];
        public static string TenantId { get;  } = ConfigurationManager.AppSettings["ida:TenantId"];

        public static string Authority { get; } = AADInstance + TenantId;
    }
}