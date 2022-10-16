namespace Pantree.Store
{
    public static class AppConfig
    {
        private static ConfigurationManager? Configuration { get; set; }
        public static string ConnectionString => Configuration.GetValue<string>("ConnectionString");
        public static string CookieKey => Configuration.GetValue<string>("CookieKey");

        public static void SetConfiguration(ConfigurationManager configuration)
        {
            if (Configuration == null) Configuration = configuration;
        }
    }
}