using Microsoft.Playwright;
using PlaywrightAutomation.Configuration;

namespace PlaywrightAutomation.Pages;

public class LoginPage : BasePage
{
    // Selectors
    private const string UsernameInput = "input[name='username']";
    private const string PasswordInput = "input[name='password']";
    private const string LoginButton = "button[type='submit']";
    private const string DashboardHeader = ".page-header h1";

    public LoginPage(IPage page) : base(page) { }

    public async Task NavigateToLoginAsync()
    {
        await NavigateToAsync(TestConfiguration.Urls.SnipeItLogin);
    }

    public async Task LoginAsync(string username, string password)
    {
        await FillAsync(UsernameInput, username);
        await FillAsync(PasswordInput, password);
        await ClickAsync(LoginButton);
        
        // Wait for successful login - dashboard should load
        await WaitForElementAsync(DashboardHeader);
    }

    public async Task<bool> IsLoginSuccessfulAsync()
    {
        return await IsVisibleAsync(DashboardHeader);
    }
}
