using Microsoft.Playwright;
using PlaywrightAutomation.Models;

namespace PlaywrightAutomation.Pages;

public class CreateAssetPage : BasePage
{
    // Selectors
    private const string AssetTagInput = "input[name='asset_tag']";
    private const string ModelSelect = "select[name='model_id']";
    private const string StatusSelect = "select[name='status_id']";
    private const string CheckoutToSelect = "select[name='assigned_user']";
    private const string SerialNumberInput = "input[name='serial']";
    private const string NotesTextarea = "textarea[name='notes']";
    private const string SaveButton = "button[type='submit']";
    private const string SuccessAlert = ".alert-success";

    public CreateAssetPage(IPage page) : base(page) { }

    public async Task FillAssetDetailsAsync(Asset asset)
    {
        if (!string.IsNullOrEmpty(asset.AssetTag))
        {
            await FillAsync(AssetTagInput, asset.AssetTag);
        }

        if (!string.IsNullOrEmpty(asset.SerialNumber))
        {
            await FillAsync(SerialNumberInput, asset.SerialNumber);
        }

        if (!string.IsNullOrEmpty(asset.Notes))
        {
            await FillAsync(NotesTextarea, asset.Notes);
        }
    }

    public async Task SelectModelAsync(string modelName)
    {
        // Wait for the model dropdown to be populated
        await Page.WaitForTimeoutAsync(2000);
        
        var modelOptions = Page.Locator($"{ModelSelect} option");
        var count = await modelOptions.CountAsync();
        
        for (int i = 0; i < count; i++)
        {
            var option = modelOptions.Nth(i);
            var text = await option.TextContentAsync();
            if (text != null && text.Contains(modelName))
            {
                var value = await option.GetAttributeAsync("value");
                if (!string.IsNullOrEmpty(value))
                {
                    await SelectOptionAsync(ModelSelect, value);
                    return;
                }
            }
        }
        
        throw new InvalidOperationException($"Model '{modelName}' not found in dropdown");
    }

    public async Task SelectStatusAsync(string statusName)
    {
        await Page.WaitForTimeoutAsync(1000);
        
        var statusOptions = Page.Locator($"{StatusSelect} option");
        var count = await statusOptions.CountAsync();
        
        for (int i = 0; i < count; i++)
        {
            var option = statusOptions.Nth(i);
            var text = await option.TextContentAsync();
            if (text != null && text.Contains(statusName))
            {
                var value = await option.GetAttributeAsync("value");
                if (!string.IsNullOrEmpty(value))
                {
                    await SelectOptionAsync(StatusSelect, value);
                    return;
                }
            }
        }
        
        throw new InvalidOperationException($"Status '{statusName}' not found in dropdown");
    }

    public async Task SelectRandomUserAsync()
    {
        await Page.WaitForTimeoutAsync(1000);
        
        var userOptions = Page.Locator($"{CheckoutToSelect} option");
        var count = await userOptions.CountAsync();
        
        if (count > 1) // Skip the first empty option
        {
            var randomIndex = new Random().Next(1, count);
            var option = userOptions.Nth(randomIndex);
            var value = await option.GetAttributeAsync("value");
            if (!string.IsNullOrEmpty(value))
            {
                await SelectOptionAsync(CheckoutToSelect, value);
            }
        }
    }

    public async Task SubmitAssetAsync()
    {
        await ClickAsync(SaveButton);
        
        // Wait for either success message or redirect
        try
        {
            await WaitForElementAsync(SuccessAlert, 5000);
        }
        catch
        {
            // If no success alert, assume redirect happened (which is also success)
        }
    }

    public async Task<bool> IsSuccessMessageVisibleAsync()
    {
        return await IsVisibleAsync(SuccessAlert);
    }

    public async Task CreateAssetAsync(Asset asset)
    {
        await FillAssetDetailsAsync(asset);
        
        if (!string.IsNullOrEmpty(asset.Model))
        {
            await SelectModelAsync(asset.Model);
        }
        
        if (!string.IsNullOrEmpty(asset.Status))
        {
            await SelectStatusAsync(asset.Status);
        }
        
        // Always select a random user for checkout
        await SelectRandomUserAsync();
        
        await SubmitAssetAsync();
    }
}
