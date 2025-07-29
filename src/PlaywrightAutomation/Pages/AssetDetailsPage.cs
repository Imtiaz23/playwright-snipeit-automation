using Microsoft.Playwright;
using PlaywrightAutomation.Models;

namespace PlaywrightAutomation.Pages;

public class AssetDetailsPage : BasePage
{
    // Selectors
    private const string AssetTagLabel = "dt:has-text('Asset Tag') + dd";
    private const string ModelLabel = "dt:has-text('Model') + dd";
    private const string StatusLabel = "dt:has-text('Status') + dd";
    private const string CheckedOutToLabel = "dt:has-text('Checked Out To') + dd";
    private const string SerialNumberLabel = "dt:has-text('Serial') + dd";
    private const string NotesLabel = "dt:has-text('Notes') + dd";
    private const string HistoryTab = "a[href*='#history']";
    private const string HistoryTabContent = "#history";
    private const string HistoryTable = "#history table";
    private const string HistoryRows = "#history table tbody tr";

    public AssetDetailsPage(IPage page) : base(page) { }

    public async Task<string?> GetAssetTagAsync()
    {
        return await GetTextAsync(AssetTagLabel);
    }

    public async Task<string?> GetModelAsync()
    {
        return await GetTextAsync(ModelLabel);
    }

    public async Task<string?> GetStatusAsync()
    {
        return await GetTextAsync(StatusLabel);
    }

    public async Task<string?> GetCheckedOutToAsync()
    {
        return await GetTextAsync(CheckedOutToLabel);
    }

    public async Task<string?> GetSerialNumberAsync()
    {
        return await GetTextAsync(SerialNumberLabel);
    }

    public async Task<string?> GetNotesAsync()
    {
        return await GetTextAsync(NotesLabel);
    }

    public async Task<Asset> GetAssetDetailsAsync()
    {
        return new Asset
        {
            AssetTag = await GetAssetTagAsync(),
            Model = await GetModelAsync(),
            Status = await GetStatusAsync(),
            CheckedOutTo = await GetCheckedOutToAsync(),
            SerialNumber = await GetSerialNumberAsync(),
            Notes = await GetNotesAsync()
        };
    }

    public async Task ClickHistoryTabAsync()
    {
        await ClickAsync(HistoryTab);
        await WaitForElementAsync(HistoryTabContent);
    }

    public async Task<List<string>> GetHistoryEntriesAsync()
    {
        await ClickHistoryTabAsync();
        
        var historyEntries = new List<string>();
        var historyRows = Page.Locator(HistoryRows);
        var count = await historyRows.CountAsync();
        
        for (int i = 0; i < count; i++)
        {
            var row = historyRows.Nth(i);
            var text = await row.TextContentAsync();
            if (!string.IsNullOrEmpty(text))
            {
                historyEntries.Add(text.Trim());
            }
        }
        
        return historyEntries;
    }

    public async Task<bool> ValidateAssetCreationInHistoryAsync()
    {
        var historyEntries = await GetHistoryEntriesAsync();
        
        // Look for creation or checkout entries in history
        return historyEntries.Any(entry => 
            entry.Contains("created", StringComparison.OrdinalIgnoreCase) ||
            entry.Contains("checked out", StringComparison.OrdinalIgnoreCase) ||
            entry.Contains("asset was created", StringComparison.OrdinalIgnoreCase));
    }

    public async Task<bool> ValidateAssetDetailsAsync(Asset expectedAsset)
    {
        var actualAsset = await GetAssetDetailsAsync();
        
        bool isValid = true;
        
        if (!string.IsNullOrEmpty(expectedAsset.AssetTag))
        {
            isValid &= actualAsset.AssetTag?.Contains(expectedAsset.AssetTag) == true;
        }
        
        if (!string.IsNullOrEmpty(expectedAsset.Model))
        {
            isValid &= actualAsset.Model?.Contains(expectedAsset.Model) == true;
        }
        
        if (!string.IsNullOrEmpty(expectedAsset.Status))
        {
            isValid &= actualAsset.Status?.Contains(expectedAsset.Status) == true;
        }
        
        if (!string.IsNullOrEmpty(expectedAsset.SerialNumber))
        {
            isValid &= actualAsset.SerialNumber?.Contains(expectedAsset.SerialNumber) == true;
        }
        
        return isValid;
    }
}
