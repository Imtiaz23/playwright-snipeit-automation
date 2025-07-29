using System.Threading.Tasks;
using Microsoft.Playwright;

namespace PlaywrightAutomation.Pages;

public abstract class BasePage
{
    protected readonly IPage Page;

    protected BasePage(IPage page)
    {
        Page = page;
    }

    protected async Task NavigateToAsync(string url) => await Page.GotoAsync(url);
    protected async Task ClickAsync(string selector) => await Page.ClickAsync(selector);
    protected async Task FillAsync(string selector, string value) => await Page.FillAsync(selector, value);
    protected async Task WaitForElementAsync(string selector, int timeout = 30000) => 
        await Page.WaitForSelectorAsync(selector, new() { Timeout = timeout });
    protected async Task<bool> IsVisibleAsync(string selector) => await Page.IsVisibleAsync(selector);
}
