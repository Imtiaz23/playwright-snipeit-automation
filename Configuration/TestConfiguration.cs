namespace PlaywrightAutomation.Configuration;

public static class TestConfiguration
{
    public static class Urls
    {
        public const string SnipeItLogin = "https://demo.snipeitapp.com/login";
        public const string SnipeItBase = "https://demo.snipeitapp.com";
        public const string SnipeItAssets = "https://demo.snipeitapp.com/hardware";
    }

    public static class Credentials
    {
        public const string Username = "admin";
        public const string Password = "password";
    }

    public static class AssetDefaults
    {
        public const string Model = "MacBook Pro 13\"";
        public const string Status = "Ready to Deploy";
        public const string Manufacturer = "Apple";
        public const string Category = "Laptop";
    }

    public static class Timeouts
    {
        public const int DefaultTimeout = 30000;
        public const int ShortTimeout = 5000;
        public const int LongTimeout = 60000;
    }
}
