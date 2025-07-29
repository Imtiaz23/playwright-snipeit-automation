using Microsoft.Playwright;
using NUnit.Framework;
using PlaywrightAutomation.Configuration;

namespace PlaywrightAutomation.Tests;

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
            SlowMo = 1000 // Increased delay for better visibility during debugging
        });
        
        BrowserContext = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
            RecordVideoDir = "videos/",
            RecordVideoSize = new RecordVideoSize { Width = 1920, Height = 1080 }
        });
        
        // Enable request/response logging for debugging
        BrowserContext.Request += (sender, request) => 
        {
            Console.WriteLine($"Request: {request.Method} {request.Url}");
        };
        
        Page = await BrowserContext.NewPageAsync();
        
        // Set default timeouts
        Page.SetDefaultTimeout(TestConfiguration.Timeouts.DefaultTimeout);
        Page.SetDefaultNavigationTimeout(TestConfiguration.Timeouts.DefaultTimeout);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDownAsync()
    {
        if (Page != null)
        {
            await Page.CloseAsync();
        }
        
        if (BrowserContext != null)
        {
            await BrowserContext.CloseAsync();
        }
        
        if (Browser != null)
        {
            await Browser.CloseAsync();
        }
        
        Playwright?.Dispose();
    }

    [SetUp]
    public async Task SetUpAsync()
    {
        // Clear any existing sessions/cookies before each test
        if (BrowserContext != null)
        {
            await BrowserContext.ClearCookiesAsync();
        }
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        // Take screenshot on test failure
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

    protected async Task NavigateToAsync(string url)
    {
        if (Page == null) throw new InvalidOperationException("Page is not initialized");
        await Page.GotoAsync(url);
    }

    protected async Task WaitForElementAsync(string selector)
    {
        if (Page == null) throw new InvalidOperationException("Page is not initialized");
        await Page.WaitForSelectorAsync(selector);
    }
}