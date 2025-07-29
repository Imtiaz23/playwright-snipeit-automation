using Microsoft.Playwright;
using PlaywrightAutomation.Configuration;

namespace PlaywrightAutomation.Pages;

public abstract class BasePage
{
    protected readonly IPage Page;

    protected BasePage(IPage page)
    {
        Page = page;
    }

    public virtual async Task NavigateToAsync(string url)
    {
        await Page.GotoAsync(url, new PageGotoOptions 
        { 
            WaitUntil = WaitUntilState.DOMContentLoaded,
            Timeout = TestConfiguration.Timeouts.DefaultTimeout
        });
    }

    public virtual async Task WaitForElementAsync(string selector, int timeout = TestConfiguration.Timeouts.DefaultTimeout)
    {
        await Page.Locator(selector).WaitForAsync(new LocatorWaitForOptions 
        { 
            Timeout = timeout 
        });
    }

    public virtual async Task ClickAsync(string selector)
    {
        await Page.Locator(selector).ClickAsync();
    }

    public virtual async Task FillAsync(string selector, string value)
    {
        await Page.Locator(selector).FillAsync(value);
    }

    public virtual async Task<string?> GetTextAsync(string selector)
    {
        return await Page.Locator(selector).TextContentAsync();
    }

    public virtual async Task<bool> IsVisibleAsync(string selector)
    {
        return await Page.Locator(selector).IsVisibleAsync();
    }

    public virtual async Task SelectOptionAsync(string selector, string value)
    {
        await Page.Locator(selector).SelectOptionAsync(value);
    }

    public virtual async Task WaitForUrlAsync(string urlPattern)
    {
        await Page.WaitForURLAsync(urlPattern, new PageWaitForURLOptions 
        { 
            Timeout = TestConfiguration.Timeouts.DefaultTimeout 
        });
    }
}