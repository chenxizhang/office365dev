namespace Office365GraphCoreMVCHelper
{
    public class AppSetting
    {

        public Info Office365ApplicationInfo { get; set; }

        public class Info
        {
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
            public string Authority { get; set; }

            public string GraphResourceId { get; set; }
        }
    }
}
