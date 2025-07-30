using System.Threading.Tasks;
using Microsoft.Playwright;
using PlaywrightAutomation.Configuration;

namespace PlaywrightAutomation.Pages;

public class AssetsPage : BasePage
{
    private const string AssetTable = ".table-responsive table";

    public AssetsPage(IPage page) : base(page) { }

    public async Task<bool> IsOnAssetsPageAsync()
    {
        return await IsVisibleAsync(AssetTable);
    }
}
