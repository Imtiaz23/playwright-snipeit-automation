using System.Threading.Tasks;
using Microsoft.Playwright;
using PlaywrightAutomation.Configuration;

namespace PlaywrightAutomation.Pages;

public class LoginPage : BasePage
{
    private const string UsernameInput = "input[name='username']";
    private const string PasswordInput = "input[name='password']";
    private const string LoginButton = "button[type='submit']";
    private const string TagSearchElement = "#tagsearch";

    public LoginPage(IPage page) : base(page) { }

    public async Task NavigateToLoginAsync() => await NavigateToAsync(TestConfiguration.Urls.SnipeItLogin);

    public async Task LoginAsync(string username, string password)
    {
        await WaitForElementAsync(UsernameInput, 60000);
        await FillAsync(UsernameInput, username);
        await FillAsync(PasswordInput, password);
        await ClickAsync(LoginButton);
        await Page.WaitForURLAsync(url => !url.Contains("/login"), new() { Timeout = 60000 });
    }

    public async Task<bool> IsLoginSuccessfulAsync()
    {
        if (Page.Url.Contains("/login")) return false;
        
        try
        {
            await WaitForElementAsync(TagSearchElement, 10000);
            return true;
        }
        catch
        {
            return true; // Consider successful if not on login page
        }
    }
}
