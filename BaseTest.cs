using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;
using PlaywrightAutomation.Configuration;

namespace PlaywrightAutomation;

[TestFixture]
public abstract class BaseTest
{
    protected IPlaywright? Playwright { get; private set; }
    protected IBrowser? Browser { get; private set; }
    protected IBrowserContext? BrowserContext { get; private set; }
    protected IPage? Page { get; private set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUpAsync()
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        
        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions 
        { 
            Headless = false,
            SlowMo = 1000
        });
        
        BrowserContext = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
            RecordVideoDir = Path.Combine("bin", "Debug", "net8.0", "videos"),
            RecordVideoSize = new RecordVideoSize { Width = 1920, Height = 1080 }
        });
        
        Page = await BrowserContext.NewPageAsync();
        Page.SetDefaultTimeout(TestConfiguration.Timeouts.DefaultTimeout);
        Page.SetDefaultNavigationTimeout(TestConfiguration.Timeouts.DefaultTimeout);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDownAsync()
    {
        await Page?.CloseAsync();
        await BrowserContext?.CloseAsync();
        await Browser?.CloseAsync();
        Playwright?.Dispose();
    }

    [SetUp]
    public async Task SetUpAsync()
    {
        Directory.CreateDirectory("screenshots");
        Directory.CreateDirectory(Path.Combine("bin", "Debug", "net8.0", "videos"));
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            if (Page != null)
            {
                var screenshot = await Page.ScreenshotAsync();
                var testName = TestContext.CurrentContext.Test.Name;
                var fileName = $"screenshot-{testName}-{DateTime.Now:yyyyMMdd-HHmmss}.png";
                await File.WriteAllBytesAsync(Path.Combine("screenshots", fileName), screenshot);
                Console.WriteLine($"Screenshot saved: {fileName}");
            }
        }
    }
}
