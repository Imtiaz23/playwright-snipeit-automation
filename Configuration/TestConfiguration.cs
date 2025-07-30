namespace PlaywrightAutomation.Configuration;

public static class TestConfiguration
{
    public static class Urls
    {
        public const string SnipeItLogin = "https://demo.snipeitapp.com/login";
        public const string SnipeItAssets = "https://demo.snipeitapp.com/hardware";
    }

    public static class Timeouts
    {
        public const int DefaultTimeout = 30000;
    }
}
