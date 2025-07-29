using Microsoft.Playwright;
using PlaywrightAutomation.Configuration;
using PlaywrightAutomation.Models;

namespace PlaywrightAutomation.Pages;

public class AssetsPage : BasePage
{
    // Selectors
    private const string CreateAssetButton = "a[href*='hardware/create']";
    private const string AssetTable = ".table-responsive table";
    private const string AssetRows = "tbody tr";
    private const string SearchInput = "input[type='search']";
    private const string AssetLink = "td a[href*='/hardware/']";

    public AssetsPage(IPage page) : base(page) { }

    public async Task NavigateToAssetsAsync()
    {
        await NavigateToAsync(TestConfiguration.Urls.SnipeItAssets);
        await WaitForElementAsync(AssetTable);
    }

    public async Task ClickCreateAssetAsync()
    {
        await ClickAsync(CreateAssetButton);
    }

    public async Task SearchForAssetAsync(string searchTerm)
    {
        await FillAsync(SearchInput, searchTerm);
        await Page.Keyboard.PressAsync("Enter");
        
        // Wait for search results to load
        await Page.WaitForTimeoutAsync(2000);
    }

    public async Task<bool> IsAssetVisibleInListAsync(string assetTag)
    {
        await SearchForAssetAsync(assetTag);
        
        var assetRows = Page.Locator(AssetRows);
        var count = await assetRows.CountAsync();
        
        for (int i = 0; i < count; i++)
        {
            var row = assetRows.Nth(i);
            var text = await row.TextContentAsync();
            if (text != null && text.Contains(assetTag))
            {
                return true;
            }
        }
        
        return false;
    }

    public async Task ClickAssetByTagAsync(string assetTag)
    {
        await SearchForAssetAsync(assetTag);
        
        var assetRows = Page.Locator(AssetRows);
        var count = await assetRows.CountAsync();
        
        for (int i = 0; i < count; i++)
        {
            var row = assetRows.Nth(i);
            var text = await row.TextContentAsync();
            if (text != null && text.Contains(assetTag))
            {
                var link = row.Locator(AssetLink).First;
                await link.ClickAsync();
                return;
            }
        }
        
        throw new InvalidOperationException($"Asset with tag {assetTag} not found in the list");
    }

    public async Task<List<string>> GetAssetTagsFromCurrentPageAsync()
    {
        var assetTags = new List<string>();
        var assetRows = Page.Locator(AssetRows);
        var count = await assetRows.CountAsync();
        
        for (int i = 0; i < count; i++)
        {
            var row = assetRows.Nth(i);
            var firstCell = row.Locator("td").First;
            var text = await firstCell.TextContentAsync();
            if (!string.IsNullOrEmpty(text))
            {
                assetTags.Add(text.Trim());
            }
        }
        
        return assetTags;
    }
}
