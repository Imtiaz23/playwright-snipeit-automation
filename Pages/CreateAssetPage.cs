using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using PlaywrightAutomation.Models;

namespace PlaywrightAutomation.Pages;

public class CreateAssetPage : BasePage
{
    private const string AssetTagInput = ".form-group #asset_tag";
    private const string ModelSelect = "select[name='model_id']";
    private const string StatusSelect = "select[name='status_id']";
    private const string CheckoutToSelect = "select[name='assigned_user']";
    private const string SerialNumberInput = "input[name='serial']";
    private const string SerialInput = "input[name='serial']";
    private const string PurchaseDateInput = "input[name='purchase_date']";
    private const string PurchaseCostInput = "input[name='purchase_cost']";
    private const string NotesTextarea = "textarea[name='notes']";
    private const string SaveButton = "button[type='submit']";
    private const string SuccessAlert = ".alert-success";

    public CreateAssetPage(IPage page) : base(page) { }

    public async Task CreateAssetAsync(Asset asset)
    {
        if (!string.IsNullOrEmpty(asset.AssetTag))
            await FillAsync(AssetTagInput, asset.AssetTag);
        
        if (!string.IsNullOrEmpty(asset.SerialNumber))
            await FillAsync(SerialNumberInput, asset.SerialNumber);
        
        if (!string.IsNullOrEmpty(asset.Notes))
            await FillAsync(NotesTextarea, asset.Notes);

        // Select model
        await Page.WaitForTimeoutAsync(2000);
        await Page.SelectOptionAsync(ModelSelect, new SelectOptionValue { Label = "MacBook Pro 13\"" });

        // Select status
        await Page.SelectOptionAsync(StatusSelect, new SelectOptionValue { Label = "Ready to Deploy" });

        // Select random user
        var userOptions = await Page.Locator($"{CheckoutToSelect} option").AllAsync();
        if (userOptions.Count > 1)
        {
            var randomIndex = new Random().Next(1, userOptions.Count);
            var value = await userOptions[randomIndex].GetAttributeAsync("value");
            if (!string.IsNullOrEmpty(value))
                await Page.SelectOptionAsync(CheckoutToSelect, value);
        }

        await ClickAsync(SaveButton);
    }

    public async Task<bool> IsSuccessMessageVisibleAsync() => await IsVisibleAsync(SuccessAlert);

    public async Task<bool> IsOnCreateAssetPageAsync()
    {
        return await IsVisibleAsync(AssetTagInput);
    }

    public async Task FillAssetFormAsync(Asset asset)
    {
        await FillAsync(AssetTagInput, asset.AssetTag);
        await Page.SelectOptionAsync(ModelSelect, "Laptop");
        await FillAsync(SerialInput, asset.SerialNumber);
        await FillAsync(PurchaseDateInput, asset.PurchaseDate.ToString("yyyy-MM-dd"));
        await FillAsync(PurchaseCostInput, asset.PurchaseCost.ToString());
    }

    public async Task ClickSaveButtonAsync() => await ClickAsync(SaveButton);

    public async Task<bool> IsAssetCreatedAsync() => await IsSuccessMessageVisibleAsync();
}
