using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using PlaywrightAutomation.Models;

namespace PlaywrightAutomation.Pages;

public class CreateAssetPage : BasePage
{
    // Asset Tag Elements
    private const string AssetTagInput = ".form-group #asset_tag";
    
    // Model Selection Elements
    private const string ModelSelectContainer = ".select2-container[data-select2-id='10'] .select2-selection";
    private const string ModelSearchInput = ".select2-search__field";
    private const string ModelResults = "#select2-model_select_id-results";
    
    // Status Selection Elements
    private const string StatusSelectContainer = "#select2-status_select_id-container";
    private const string StatusResults = "#select2-status_select_id-results";
    
    // User Assignment Elements
    private const string AssignedUserContainer = "#select2-assigned_user_select-container";
    private const string AssignedUserResults = "#select2-assigned_user_select-results";
    
    // Other Form Elements
    private const string NotesField = "#notes";
    private const string SubmitButton = "#submit_button";
    
    // Success Verification
    private const string SuccessNotificationElement = "#success-notification";

    public CreateAssetPage(IPage page) : base(page) { }

    public async Task<string> GetGeneratedAssetTagAsync()
    {
        await WaitForElementAsync(AssetTagInput, 10000);
        var assetTagValue = await Page.GetAttributeAsync(AssetTagInput, "value");
        return assetTagValue ?? string.Empty;
    }

    public async Task SelectModelAsync()
    {
        // Step 2: Click on model select and search for MacBook Pro 13
        await WaitForElementAsync(ModelSelectContainer, 10000);
        await ClickAsync(ModelSelectContainer);
        
        await WaitForElementAsync(ModelSearchInput, 10000);
        await FillAsync(ModelSearchInput, "Macbook Pro 13");
        
        await WaitForElementAsync(ModelResults, 10000);
        await ClickAsync(ModelResults);
    }

    public async Task SelectStatusAsync()
    {
        // Step 3: Click on status select and search for Ready to Deploy
        await WaitForElementAsync(StatusSelectContainer, 10000);
        await ClickAsync(StatusSelectContainer);
        
        await WaitForElementAsync(ModelSearchInput, 10000);
        await FillAsync(ModelSearchInput, "Ready to Deploy");
        
        await WaitForElementAsync("#select2-status_select_id-results .select2-results__option", 10000);
        await ClickAsync("#select2-status_select_id-results .select2-results__option");
    }

    public async Task SelectFirstUserAsync()
    {
        // Step 4: Click on assigned user select and choose first option
        await WaitForElementAsync(AssignedUserContainer, 10000);
        await ClickAsync(AssignedUserContainer);
        
        await WaitForElementAsync("#select2-assigned_user_select-results .select2-results__option", 10000);
        
        // Click the first option in the results
        await ClickAsync("#select2-assigned_user_select-results .select2-results__option");
    }

    public async Task FillNotesAsync()
    {
        // Step 5: Fill notes field
        await WaitForElementAsync(NotesField, 10000);
        await FillAsync(NotesField, "IA Demo Test Creation");
    }

    public async Task SubmitFormAsync()
    {
        // Step 6: Click submit button
        await WaitForElementAsync(SubmitButton, 10000);
        await ClickAsync(SubmitButton);
    }

    public async Task<bool> VerifyDashboardAsync()
    {
        // Step 7: Verify successful asset creation with success notification
        try
        {
            // Wait a bit for navigation to complete
            await Page.WaitForTimeoutAsync(2000);
            
            // Wait for the success notification to appear
            await WaitForElementAsync(SuccessNotificationElement, 30000);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Success notification verification failed: {ex.Message}");
            Console.WriteLine($"Current URL: {Page.Url}");
            return false;
        }
    }

    public async Task<string> CreateAssetWithStepsAsync()
    {
        // Execute all steps in sequence
        var assetTag = await GetGeneratedAssetTagAsync();
        await SelectModelAsync();
        await SelectStatusAsync();
        await SelectFirstUserAsync();
        await FillNotesAsync();
        await SubmitFormAsync();
        
        var isSuccess = await VerifyDashboardAsync();
        if (!isSuccess)
        {
            throw new Exception("Asset creation failed - dashboard verification failed");
        }
        
        return assetTag;
    }

    public async Task<bool> IsOnCreateAssetPageAsync()
    {
        return await IsVisibleAsync(AssetTagInput);
    }
}
