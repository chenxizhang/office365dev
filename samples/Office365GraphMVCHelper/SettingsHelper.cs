using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Office365GraphMVCHelper
{
    class SettingsHelper
    {
        //public static string GraphResourceId { get; } = "https://graph.microsoft.com";
        //public static string GallatinGraphResourceId { get; } = "https://microsoftgraph.chinacloudapi.cn";

        public static string GraphAADInstance { get; } = "https://login.microsoftonline.com/";
        public static string GallatinGraphAADInstance { get; } = "https://login.chinacloudapi.cn/";

        public static string AADInstance { get; } = ConfigurationManager.AppSettings["ida:AADInstance"];
        public static string ClientId { get;  } = ConfigurationManager.AppSettings["ida:ClientId"];
        public static string ClientSecret { get;  } = ConfigurationManager.AppSettings["ida:ClientSecret"];
        public static string TenantId { get;  } = ConfigurationManager.AppSettings["ida:TenantId"];

        public static string GraphResourceId
        {
            get
            {
                var result = ConfigurationManager.AppSettings["ida:ResourceId"];
                if (string.IsNullOrEmpty(result))
                {
                    result = "https://graph.microsoft.com";
                }
                return result;
            }
        }

        public static string Authority { get; } = AADInstance + TenantId;
    }
}