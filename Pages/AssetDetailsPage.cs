using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using PlaywrightAutomation.Models;

namespace PlaywrightAutomation.Pages;

public class AssetDetailsPage : BasePage
{
    private const string AssetTagLabel = "dt:has-text('Asset Tag') + dd";
    private const string AssetTag = "dt:has-text('Asset Tag') + dd";
    private const string ModelLabel = "dt:has-text('Model') + dd";
    private const string StatusLabel = "dt:has-text('Status') + dd";
    private const string CheckedOutToLabel = "dt:has-text('Checked Out To') + dd";
    private const string DeleteButton = "a[data-tooltip='Delete']";
    private const string ConfirmDeleteButton = "button:has-text('Delete')";
    private const string HistoryTable = ".table-striped";
    private const string SerialNumberLabel = "dt:has-text('Serial') + dd";
    private const string HistoryTab = "a[href*='#history']";
    private const string HistoryRows = "#history table tbody tr";

    public AssetDetailsPage(IPage page) : base(page) { }

    public async Task<bool> IsOnAssetDetailsPageAsync()
    {
        return await IsVisibleAsync(AssetTag);
    }

    public async Task<string> GetAssetTagAsync()
    {
        return await Page.Locator(AssetTag).TextContentAsync() ?? "";
    }

    public async Task ClickDeleteButtonAsync()
    {
        await ClickAsync(DeleteButton);
    }

    public async Task ConfirmDeletionAsync()
    {
        await ClickAsync(ConfirmDeleteButton);
    }

    public async Task<bool> IsAssetDeletedAsync()
    {
        // Check if we're redirected back to assets list
        await Page.WaitForTimeoutAsync(2000);
        return Page.Url.Contains("/hardware") && !Page.Url.Contains("/view/");
    }

    public async Task<Asset> GetAssetDetailsAsync()
    {
        return new Asset
        {
            AssetTag = await Page.Locator(AssetTagLabel).TextContentAsync(),
            Model = await Page.Locator(ModelLabel).TextContentAsync(),
            Status = await Page.Locator(StatusLabel).TextContentAsync(),
            CheckedOutTo = await Page.Locator(CheckedOutToLabel).TextContentAsync(),
            SerialNumber = await Page.Locator(SerialNumberLabel).TextContentAsync()
        };
    }

    public async Task<bool> ValidateAssetDetailsAsync(Asset expectedAsset)
    {
        var actualAsset = await GetAssetDetailsAsync();
        return actualAsset.AssetTag?.Contains(expectedAsset.AssetTag ?? "") == true &&
               actualAsset.Model?.Contains(expectedAsset.Model ?? "") == true;
    }

    public async Task<List<string>> GetHistoryEntriesAsync()
    {
        await ClickAsync(HistoryTab);
        await WaitForElementAsync(HistoryRows);
        
        var entries = new List<string>();
        var rows = Page.Locator(HistoryRows);
        var count = await rows.CountAsync();
        
        for (int i = 0; i < count; i++)
        {
            var text = await rows.Nth(i).TextContentAsync();
            if (!string.IsNullOrEmpty(text)) entries.Add(text.Trim());
        }
        return entries;
    }

    public async Task<bool> ValidateAssetCreationInHistoryAsync()
    {
        var entries = await GetHistoryEntriesAsync();
        return entries.Any(entry => entry.Contains("created") || entry.Contains("checked out"));
    }
}
