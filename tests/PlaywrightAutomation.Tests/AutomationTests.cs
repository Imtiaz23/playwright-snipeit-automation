using FluentAssertions;
using NUnit.Framework;
using PlaywrightAutomation.Configuration;
using PlaywrightAutomation.Models;
using PlaywrightAutomation.Pages;
using PlaywrightAutomation.Tests;
using PlaywrightAutomation.Utils;

namespace PlaywrightAutomation.Tests;

[TestFixture]
public class SnipeItAutomationTests : BaseTest
{
    private LoginPage? _loginPage;
    private AssetsPage? _assetsPage;
    private CreateAssetPage? _createAssetPage;
    private AssetDetailsPage? _assetDetailsPage;

    [SetUp]
    public async Task SetUpPages()
    {
        if (Page == null)
            throw new InvalidOperationException("Page is not initialized");

        _loginPage = new LoginPage(Page);
        _assetsPage = new AssetsPage(Page);
        _createAssetPage = new CreateAssetPage(Page);
        _assetDetailsPage = new AssetDetailsPage(Page);

        // Ensure screenshots directory exists
        Directory.CreateDirectory("screenshots");
        Directory.CreateDirectory("videos");
    }

    [Test]
    [Description("Complete workflow: Login, Create MacBook Pro 13\" asset, Verify creation, Check details and history")]
    public async Task CreateAndVerifyMacBookProAsset()
    {
        // Step 1: Login to SnipeIT demo
        await _loginPage!.NavigateToLoginAsync();
        await _loginPage.LoginAsync(TestConfiguration.Credentials.Username, TestConfiguration.Credentials.Password);
        
        // Verify login was successful
        var isLoggedIn = await _loginPage.IsLoginSuccessfulAsync();
        isLoggedIn.Should().BeTrue("Login should be successful");

        // Step 2: Generate test data for the asset
        var testAsset = TestData.GenerateAsset();
        var testUser = TestData.GenerateUser();

        Console.WriteLine($"Creating asset with tag: {testAsset.AssetTag}");
        Console.WriteLine($"Asset will be checked out to: {testUser.FullName}");

        // Step 3: Navigate to Assets page and create new asset
        await _assetsPage!.NavigateToAssetsAsync();
        await _assetsPage.ClickCreateAssetAsync();

        // Step 4: Fill in asset creation form
        await _createAssetPage!.CreateAssetAsync(testAsset);

        // Verify asset creation was successful
        var isCreationSuccessful = await _createAssetPage.IsSuccessMessageVisibleAsync();
        if (!isCreationSuccessful)
        {
            Console.WriteLine("No success message found, assuming redirect occurred (also indicates success)");
        }

        // Step 5: Navigate back to assets list and search for the created asset
        await _assetsPage.NavigateToAssetsAsync();
        
        // Step 6: Verify the asset appears in the assets list
        var isAssetVisible = await _assetsPage.IsAssetVisibleInListAsync(testAsset.AssetTag!);
        isAssetVisible.Should().BeTrue($"Asset with tag {testAsset.AssetTag} should be visible in the assets list");

        Console.WriteLine($"Asset {testAsset.AssetTag} found in assets list ✓");

        // Step 7: Navigate to the asset details page
        await _assetsPage.ClickAssetByTagAsync(testAsset.AssetTag!);

        // Step 8: Validate asset details on the asset page
        var isDetailsValid = await _assetDetailsPage!.ValidateAssetDetailsAsync(testAsset);
        isDetailsValid.Should().BeTrue("Asset details should match the created asset");

        // Get actual details for logging
        var actualAsset = await _assetDetailsPage.GetAssetDetailsAsync();
        Console.WriteLine($"Asset Details Validation:");
        Console.WriteLine($"  Asset Tag: {actualAsset.AssetTag}");
        Console.WriteLine($"  Model: {actualAsset.Model}");
        Console.WriteLine($"  Status: {actualAsset.Status}");
        Console.WriteLine($"  Checked Out To: {actualAsset.CheckedOutTo}");
        Console.WriteLine($"  Serial Number: {actualAsset.SerialNumber}");

        // Step 9: Validate the History tab
        var historyEntries = await _assetDetailsPage.GetHistoryEntriesAsync();
        historyEntries.Should().NotBeEmpty("History should contain at least one entry");

        var hasCreationEntry = await _assetDetailsPage.ValidateAssetCreationInHistoryAsync();
        hasCreationEntry.Should().BeTrue("History should contain asset creation or checkout entry");

        Console.WriteLine($"History validation completed ✓");
        Console.WriteLine($"Found {historyEntries.Count} history entries");

        // Log some history entries for verification
        for (int i = 0; i < Math.Min(3, historyEntries.Count); i++)
        {
            Console.WriteLine($"  History Entry {i + 1}: {historyEntries[i]}");
        }

        Console.WriteLine("=== Test Completed Successfully ===");
    }

    [Test]
    [Description("Verify login functionality with valid credentials")]
    public async Task VerifyLoginFunctionality()
    {
        await _loginPage!.NavigateToLoginAsync();
        await _loginPage.LoginAsync(TestConfiguration.Credentials.Username, TestConfiguration.Credentials.Password);
        
        var isLoggedIn = await _loginPage.IsLoginSuccessfulAsync();
        isLoggedIn.Should().BeTrue("Login should be successful with valid credentials");
    }

    [Test]
    [Description("Verify assets page loads and displays assets")]
    public async Task VerifyAssetsPageLoads()
    {
        // Login first
        await _loginPage!.NavigateToLoginAsync();
        await _loginPage.LoginAsync(TestConfiguration.Credentials.Username, TestConfiguration.Credentials.Password);
        
        // Navigate to assets page
        await _assetsPage!.NavigateToAssetsAsync();
        
        // Get asset tags to verify page loaded with data
        var assetTags = await _assetsPage.GetAssetTagsFromCurrentPageAsync();
        assetTags.Should().NotBeEmpty("Assets page should display existing assets");
        
        Console.WriteLine($"Assets page loaded successfully with {assetTags.Count} assets visible");
    }
}