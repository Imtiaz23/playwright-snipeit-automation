using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using Microsoft.Playwright;
using PlaywrightAutomation.Pages;
using PlaywrightAutomation.Utils;
using PlaywrightAutomation.Models;

namespace PlaywrightAutomation;

[TestFixture]
public class SnipeITTests : BaseTest
{
    private LoginPage? loginPage;
    private AssetsPage? assetsPage;
    private CreateAssetPage? createAssetPage;
    private AssetDetailsPage? assetDetailsPage;
    private string? createdAssetTag;

    [SetUp]
    public async Task ClassSetUpAsync()
    {
        // Initialize page objects only once when Page is available
        if (loginPage == null)
        {
            loginPage = new LoginPage(Page!);
            assetsPage = new AssetsPage(Page!);
            createAssetPage = new CreateAssetPage(Page!);
            assetDetailsPage = new AssetDetailsPage(Page!);
        }
    }

    [Test, Order(1)]
    public async Task Test1_Login()
    {
        Console.WriteLine("Starting Test 1: Login");
        
        await loginPage!.NavigateToLoginAsync();
        await loginPage.LoginAsync("admin", "password");
        
        var isLoginSuccessful = await loginPage.IsLoginSuccessfulAsync();
        isLoginSuccessful.Should().BeTrue("Login should be successful and redirect to dashboard");
        
        Console.WriteLine("Test 1 completed: Login successful");
    }

    [Test, Order(2)]
    public async Task Test2_NavigateToCreateAsset()
    {
        Console.WriteLine("Starting Test 2: Navigate to Create Asset");
        
        // Wait for the "Create New" dropdown button to be visible before clicking
        await Page!.WaitForSelectorAsync("a.dropdown-toggle[data-toggle='dropdown']:has-text('Create New')", new() { State = WaitForSelectorState.Visible });
        await Page.ClickAsync("a.dropdown-toggle[data-toggle='dropdown']:has-text('Create New')");
        
        // Wait for dropdown menu to appear and Asset option to be visible before clicking
        await Page.WaitForSelectorAsync("a[href*='hardware/create']:has-text('Asset')", new() { State = WaitForSelectorState.Visible });
        await Page.ClickAsync("a[href*='hardware/create']:has-text('Asset')");
        
        // Wait for navigation to complete
        await Page.WaitForLoadStateAsync();
        
        // Verify we're on the create asset page
        var isOnCreateAssetPage = await createAssetPage!.IsOnCreateAssetPageAsync();
        isOnCreateAssetPage.Should().BeTrue("Should successfully navigate to create asset page");
        
        Console.WriteLine("Test 2 completed: Successfully navigated to Create Asset page");
    }

    [Test, Order(3)]
    public async Task Test3_CreateAsset()
    {
        Console.WriteLine("Starting Test 3: Create Asset");
        
        // We should already be on the create asset page from Test2
        var isOnCreateAssetPage = await createAssetPage!.IsOnCreateAssetPageAsync();
        isOnCreateAssetPage.Should().BeTrue("Should be on create asset page from previous test");
        
        // Execute the detailed asset creation steps and save the generated asset tag
        createdAssetTag = await createAssetPage.CreateAssetWithStepsAsync();
        
        // Verify asset tag was captured
        createdAssetTag.Should().NotBeNullOrEmpty("Asset tag should be generated and captured");
        
        Console.WriteLine($"Test 3 completed: Asset created with tag {createdAssetTag}");
    }

    [Test, Order(4)]
    public async Task Test4_ViewAssetDetails()
    {
        Console.WriteLine("Starting Test 4: View Asset Details");
        
        createdAssetTag.Should().NotBeNullOrEmpty("Asset tag should be available from previous test");
        
        // Verify the asset appears in recent activity on the dashboard
        // Expected format: "({assetTag}) - Macbook Pro 13""
        // Based on the provided element structure: 
        // <a href="https://demo.snipeitapp.com/hardware/2619" data-tooltip="true" title="" data-original-title="asset">
        //   <i class="fas fa-barcode text-blue "></i>  ({createdAssetTag}) - Macbook Pro 13"
        // </a>
        
        var recentActivitySelector = $"a[href*='/hardware/'][data-original-title='asset']:has-text('{createdAssetTag}')";
        
        try
        {
            await Page!.WaitForSelectorAsync(recentActivitySelector, new PageWaitForSelectorOptions { Timeout = 10000 });
            Console.WriteLine($"Successfully found asset {createdAssetTag} in recent activity");
            
            // Since asset tags should be unique, we should only find one matching element
            var matchingElements = await Page.Locator(recentActivitySelector).CountAsync();
            Console.WriteLine($"Found {matchingElements} elements matching asset tag {createdAssetTag}");
            
            if (matchingElements > 1)
            {
                Console.WriteLine($"WARNING: Found {matchingElements} duplicate elements for asset tag {createdAssetTag} - this should not happen as asset tags should be unique");
            }
            
            // Verify the link contains the expected text pattern in the correct format
            var linkText = await Page.Locator(recentActivitySelector).First.InnerTextAsync();
            Console.WriteLine($"Asset link text: '{linkText}'");
            
            // Check for the specific format: "({assetTag}) - Macbook Pro 13""
            linkText.Should().Contain($"({createdAssetTag})", $"Recent activity should show the asset tag in format '({createdAssetTag})'");
            linkText.Should().Contain("Macbook Pro 13", "Recent activity should show the model");
            linkText.Should().Contain($"({createdAssetTag}) - Macbook Pro 13", $"Recent activity should show the complete format '({createdAssetTag}) - Macbook Pro 13'");
            
            Console.WriteLine($"Test 4 completed: Successfully verified asset {createdAssetTag} in correct format in recent activity");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to find asset in recent activity: {ex.Message}");
            Console.WriteLine($"Current URL: {Page.Url}");
            
            // Try to debug by listing all similar elements
            var allAssetLinks = await Page.Locator("a[href*='/hardware/'][data-original-title='asset']").AllAsync();
            Console.WriteLine($"Found {allAssetLinks.Count} asset links in recent activity");
            
            for (int i = 0; i < Math.Min(3, allAssetLinks.Count); i++)
            {
                var linkText = await allAssetLinks[i].InnerTextAsync();
                Console.WriteLine($"Asset link {i + 1}: {linkText}");
            }
            
            throw;
        }
    }

    [Test, Order(5)]
    public async Task Test5_SearchAndVerifyAsset()
    {
        Console.WriteLine("Starting Test 5: Search and Verify Asset");
        
        createdAssetTag.Should().NotBeNullOrEmpty("Asset tag should be available from previous test");
        
        try
        {
            // Step 1: Navigate to the Assets page using the navigation menu
            Console.WriteLine("Step 1: Navigating to Assets page");
            var assetsPageLinkSelector = "a[href='https://demo.snipeitapp.com/hardware'][data-title='Assets']";
            await Page!.WaitForSelectorAsync(assetsPageLinkSelector, new PageWaitForSelectorOptions { Timeout = 10000 });
            await Page.ClickAsync(assetsPageLinkSelector);
            
            // Wait for navigation to assets page
            await Page.WaitForLoadStateAsync();
            Console.WriteLine("Successfully navigated to Assets page");
            
            // Step 2: Use the search box to search for our asset tag
            Console.WriteLine("Step 2: Searching for asset using search functionality");
            
            // Look for the search input in the assets page
            var searchBoxSelector = "input[name='search'], input[placeholder*='earch'], .dataTables_filter input";
            await Page.WaitForSelectorAsync(searchBoxSelector, new PageWaitForSelectorOptions { Timeout = 10000 });
            
            // Clear and enter our asset tag in the search box
            await Page.FillAsync(searchBoxSelector, "");
            await Page.FillAsync(searchBoxSelector, createdAssetTag!);
            Console.WriteLine($"Entered asset tag '{createdAssetTag}' in search box");
            
            // Submit the search (press Enter or let DataTables auto-search)
            await Page.Keyboard.PressAsync("Enter");
            
            // Wait for search results to filter - give enough time for the search to complete
            await Page.WaitForTimeoutAsync(5000); // Wait 5 seconds for search results to load
            Console.WriteLine("Search submitted and results filtered (waited 5 seconds)");
            
            // Step 3: Verify asset information in search results
            Console.WriteLine("Step 3: Verifying asset information in search results");
            
            // The table is already visible, just look for our asset in the table content
            // Look for our asset in the search results - the page is already filtered by search
            var assetRowSelector = $"td:has-text('{createdAssetTag}'), a:has-text('{createdAssetTag}')";
            await Page.WaitForSelectorAsync(assetRowSelector, new PageWaitForSelectorOptions { Timeout = 10000 });
            
            // Verify the asset tag appears in search results
            var searchResultElement = Page.Locator(assetRowSelector).First;
            var searchResultText = await searchResultElement.InnerTextAsync();
            Console.WriteLine($"Found asset in search results: {searchResultText}");
            
            // Get the full row content for verification
            var fullRowText = await searchResultElement.Locator("xpath=ancestor::tr").InnerTextAsync();
            Console.WriteLine($"Full row content: {fullRowText}");
            
            // Verify key information in the search result
            searchResultText.Should().Contain(createdAssetTag, $"Search results should contain the asset tag {createdAssetTag}");
            fullRowText.Should().Contain("Macbook Pro 13", "Search results should show the model information");
            fullRowText.Should().Contain("Ready to Deploy", "Search results should show the asset status as 'Ready to Deploy'");
            
            // Additional verification - check if asset tag link is clickable in search results
            var isVisible = await searchResultElement.IsVisibleAsync();
            isVisible.Should().BeTrue($"Asset tag {createdAssetTag} should be visible and clickable in search results");
            
            Console.WriteLine($"Test 5 completed: Successfully searched for and verified asset {createdAssetTag} in search results");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Test 5 failed: {ex.Message}");
            Console.WriteLine($"Current URL: {Page.Url}");
            
            // Debug information
            var currentPageTitle = await Page.TitleAsync();
            Console.WriteLine($"Current page title: {currentPageTitle}");
            
            // Try to capture what search elements are available
            var searchElements = await Page.Locator("input[type='text'], input[type='search']").AllAsync();
            Console.WriteLine($"Found {searchElements.Count} search input elements on page");
            
            throw;
        }
    }

    [Test, Order(6)]
    public async Task Test6_VerifyAssetDetailsPage()
    {
        Console.WriteLine("Starting Test 6: Verify Asset Details Page");
        
        createdAssetTag.Should().NotBeNullOrEmpty("Asset tag should be available from previous test");
        
        try
        {
            // Step 1: Click on the asset tag from the search result table row
            Console.WriteLine("Step 1: Clicking on asset tag from search results");
            
            // Find the asset tag link in the search results table
            var assetTagLinkSelector = $"td a:has-text('{createdAssetTag}'), a:has-text('{createdAssetTag}')";
            await Page!.WaitForSelectorAsync(assetTagLinkSelector, new PageWaitForSelectorOptions { Timeout = 10000 });
            
            // Click on the asset tag link to navigate to asset details page
            await Page.ClickAsync(assetTagLinkSelector);
            
            // Wait for navigation to asset details page
            await Page.WaitForLoadStateAsync();
            Console.WriteLine("Successfully navigated to asset details page");
            
            // Step 2: Verify .js-copy-assettag element has the text of saved asset tag
            Console.WriteLine("Step 2: Verifying asset tag in copy element");
            
            var copyAssetTagSelector = ".js-copy-assettag";
            await Page.WaitForSelectorAsync(copyAssetTagSelector, new PageWaitForSelectorOptions { Timeout = 10000 });
            
            var copyAssetTagText = await Page.Locator(copyAssetTagSelector).InnerTextAsync();
            Console.WriteLine($"Copy asset tag element text: '{copyAssetTagText}'");
            
            copyAssetTagText.Should().Contain(createdAssetTag!, $"Copy asset tag element should contain the asset tag {createdAssetTag}");
            
            // Step 3: Verify .fa-solid.fa-circle.text-blue element has the text "Ready to Deploy"
            Console.WriteLine("Step 3: Verifying status element shows 'Ready to Deploy'");
            
            var statusElementSelector = ".fa-solid.fa-circle.text-blue";
            await Page.WaitForSelectorAsync(statusElementSelector, new PageWaitForSelectorOptions { Timeout = 10000 });
            
            // The status text might be in the parent element or next sibling, so we'll check the surrounding area
            var statusElement = Page.Locator(statusElementSelector);
            var statusParentText = await statusElement.Locator("xpath=..").InnerTextAsync();
            Console.WriteLine($"Status area text: '{statusParentText}'");
            
            statusParentText.Should().Contain("Ready to Deploy", "Status area should show 'Ready to Deploy'");
            
            // Step 4: Verify model link has the text "Macbook Pro 13""
            Console.WriteLine("Step 4: Verifying model link shows 'Macbook Pro 13'");
            
            var modelLinkSelector = "a[href='https://demo.snipeitapp.com/models/1']";
            await Page.WaitForSelectorAsync(modelLinkSelector, new PageWaitForSelectorOptions { Timeout = 10000 });
            
            var modelLinkText = await Page.Locator(modelLinkSelector).InnerTextAsync();
            Console.WriteLine($"Model link text: '{modelLinkText}'");
            
            modelLinkText.Should().Contain("Macbook Pro 13", "Model link should contain 'Macbook Pro 13'");
            
            Console.WriteLine($"Test 6 completed: Successfully verified asset details page for asset {createdAssetTag}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Test 6 failed: {ex.Message}");
            Console.WriteLine($"Current URL: {Page.Url}");
            
            // Debug information
            var currentPageTitle = await Page.TitleAsync();
            Console.WriteLine($"Current page title: {currentPageTitle}");
            
            // Try to capture what elements are available on the page
            var copyElements = await Page.Locator(".js-copy-assettag").AllAsync();
            Console.WriteLine($"Found {copyElements.Count} copy asset tag elements");
            
            var statusElements = await Page.Locator(".fa-solid.fa-circle.text-blue").AllAsync();
            Console.WriteLine($"Found {statusElements.Count} status circle elements");
            
            var modelLinks = await Page.Locator("a[href*='/models/']").AllAsync();
            Console.WriteLine($"Found {modelLinks.Count} model link elements");
            
            throw;
        }
    }

    [Test, Order(7)]
    public async Task Test7_VerifyAssetHistory()
    {
        Console.WriteLine("Starting Test 7: Verify Asset History");
        
        createdAssetTag.Should().NotBeNullOrEmpty("Asset tag should be available from previous test");
        
        try
        {
            // Step 1: Click on the history icon (.fas.fa-history.fa-2x) from the current asset details page
            Console.WriteLine("Step 1: Clicking on asset history icon");
            
            // The history icon might be in a dropdown or tab, so we need to find the clickable parent element
            // Look for the parent link or button that contains the history icon
            var historyLinkSelector = "a:has(.fas.fa-history.fa-2x), button:has(.fas.fa-history.fa-2x), [data-toggle='tab']:has(.fas.fa-history.fa-2x)";
            
            // First try to find a clickable parent element with the history icon
            try 
            {
                await Page!.WaitForSelectorAsync(historyLinkSelector, new PageWaitForSelectorOptions { Timeout = 5000 });
                await Page.ClickAsync(historyLinkSelector);
                Console.WriteLine("Clicked on history link/button");
            }
            catch 
            {
                // If that doesn't work, try clicking the icon directly even if hidden (force click)
                var historyIconSelector = ".fas.fa-history.fa-2x";
                await Page.ClickAsync(historyIconSelector, new PageClickOptions { Force = true });
                Console.WriteLine("Force clicked on history icon");
            }
            
            // Wait for navigation to asset history page
            await Page.WaitForLoadStateAsync();
            Console.WriteLine("Successfully navigated to asset history page");
            
            // Step 2: Validate the history section is displayed
            Console.WriteLine("Step 2: Validating history section is displayed");
            
            // Check if we navigated to the history section (URL should contain #history)
            var currentUrl = Page.Url;
            currentUrl.Should().Contain("#history", "Should have navigated to the history section");
            
            // Wait for the specific asset history table using the correct selector
            var historyTableSelector = "#assetHistory tbody tr";
            await Page.WaitForSelectorAsync(historyTableSelector, new PageWaitForSelectorOptions { Timeout = 10000 });
            
            // Get all table rows in the asset history table
            var historyRows = await Page.Locator(historyTableSelector).AllAsync();
            Console.WriteLine($"Found {historyRows.Count} history table rows in #assetHistory");
            
            // Verify we have exactly 2 rows as mentioned
            historyRows.Count.Should().Be(2, "There should be exactly 2 table rows in the asset history");
            
            // Validate both history rows contain our asset tag and model information
            for (int i = 0; i < historyRows.Count; i++)
            {
                var rowText = await historyRows[i].InnerTextAsync();
                Console.WriteLine($"History row {i + 1} content: {rowText}");
                
                // Verify each row contains our asset tag
                rowText.Should().Contain(createdAssetTag!, $"History row {i + 1} should contain the asset tag {createdAssetTag}");
                
                // Verify each row contains the model information
                rowText.Should().Contain("Macbook Pro 13", $"History row {i + 1} should contain the model 'Macbook Pro 13'");
            }
            
            Console.WriteLine($"Test 7 completed: Successfully verified asset history for asset {createdAssetTag}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Test 7 failed: {ex.Message}");
            Console.WriteLine($"Current URL: {Page.Url}");
            
            // Debug information
            var currentPageTitle = await Page.TitleAsync();
            Console.WriteLine($"Current page title: {currentPageTitle}");
            
            // Try to capture what elements are available on the page
            var historyIcons = await Page.Locator(".fas.fa-history").AllAsync();
            Console.WriteLine($"Found {historyIcons.Count} history icon elements");
            
            var tables = await Page.Locator("table").AllAsync();
            Console.WriteLine($"Found {tables.Count} table elements on page");
            
            // Try to get any table rows if available
            var tableRows = await Page.Locator("table tbody tr").AllAsync();
            Console.WriteLine($"Found {tableRows.Count} table rows on page");
            
            throw;
        }
    }
}
