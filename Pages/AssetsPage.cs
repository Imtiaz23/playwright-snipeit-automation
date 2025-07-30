using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright;
using PlaywrightAutomation.Configuration;

namespace PlaywrightAutomation.Pages;

public class AssetsPage : BasePage
{
    private const string CreateAssetButton = "a[href*='hardware/create']";
    private const string AssetTable = ".table-responsive table";
    private const string AssetRows = "tbody tr";
    private const string SearchInput = ".pull-right.search.input-group .form-control.search-input";
    private const string AssetLink = "td a[href*='/hardware/']";
    private const string TableBody = "tbody";

    public AssetsPage(IPage page) : base(page) { }

    public async Task NavigateToAssetsAsync()
    {
        await NavigateToAsync(TestConfiguration.Urls.SnipeItAssets);
        await WaitForElementAsync(AssetTable);
    }

    public async Task ClickCreateAssetAsync() => await ClickAsync(CreateAssetButton);

    public async Task<bool> IsOnAssetsPageAsync()
    {
        return await IsVisibleAsync(AssetTable);
    }

    public async Task SearchForAssetAsync(string searchTerm)
    {
        await FillAsync(SearchInput, searchTerm);
        await Page.Keyboard.PressAsync("Enter");
        await Task.Delay(2000); // Wait for search results
    }

    public async Task ClickAssetLinkAsync(string assetTag)
    {
        var assetRow = Page.Locator($"tr:has-text('{assetTag}')");
        await assetRow.Locator("a").First.ClickAsync();
    }

    public async Task<bool> IsAssetVisibleInListAsync(string assetTag)
    {
        await FillAsync(SearchInput, assetTag);
        await Page.Keyboard.PressAsync("Enter");
        await Page.WaitForTimeoutAsync(2000);
        
        var assetRows = Page.Locator(AssetRows);
        var count = await assetRows.CountAsync();
        
        for (int i = 0; i < count; i++)
        {
            var text = await assetRows.Nth(i).TextContentAsync();
            if (text?.Contains(assetTag) == true) return true;
        }
        return false;
    }

    public async Task ClickAssetByTagAsync(string assetTag)
    {
        await FillAsync(SearchInput, assetTag);
        await Page.Keyboard.PressAsync("Enter");
        await Page.WaitForTimeoutAsync(2000);
        
        var assetRows = Page.Locator(AssetRows);
        var count = await assetRows.CountAsync();
        
        for (int i = 0; i < count; i++)
        {
            var row = assetRows.Nth(i);
            var text = await row.TextContentAsync();
            if (text?.Contains(assetTag) == true)
            {
                await row.Locator(AssetLink).First.ClickAsync();
                return;
            }
        }
        throw new InvalidOperationException($"Asset with tag {assetTag} not found");
    }

    public async Task<List<string>> GetAssetTagsFromCurrentPageAsync()
    {
        var assetTags = new List<string>();
        var assetRows = Page.Locator(AssetRows);
        var count = await assetRows.CountAsync();
        
        for (int i = 0; i < count; i++)
        {
            var text = await assetRows.Nth(i).Locator("td").First.TextContentAsync();
            if (!string.IsNullOrEmpty(text)) assetTags.Add(text.Trim());
        }
        return assetTags;
    }

    public async Task SearchForAssetWithSpecificInputAsync(string assetTag)
    {
        // Step 1: Search using the specific search input element
        await WaitForElementAsync(SearchInput, 10000);
        await FillAsync(SearchInput, assetTag);
        
        // Wait 5 seconds as specified
        await Page.WaitForTimeoutAsync(5000);
    }

    public async Task<(string assetTag, string model, string status)> VerifyAssetInTableAsync(string expectedAssetTag)
    {
        // Step 2: Verify there's only one row and check td elements
        await WaitForElementAsync(TableBody, 10000);
        var rows = Page.Locator($"{TableBody} tr");
        var rowCount = await rows.CountAsync();
        
        if (rowCount != 1)
        {
            throw new Exception($"Expected 1 row in search results, but found {rowCount}");
        }

        var row = rows.First;
        var cells = row.Locator("td");
        
        // Get the asset tag, model, and status from the table
        var assetTag = await cells.Nth(0).TextContentAsync() ?? "";
        var model = await cells.Nth(1).TextContentAsync() ?? "";
        var status = await cells.Nth(2).TextContentAsync() ?? "";
        
        return (assetTag.Trim(), model.Trim(), status.Trim());
    }

    public async Task ClickAssetTagInTableAsync()
    {
        // Step 3: Click on the asset tag shown in table data
        var firstRow = Page.Locator($"{TableBody} tr").First;
        var assetTagLink = firstRow.Locator("td").First.Locator("a");
        await assetTagLink.ClickAsync();
    }
}
