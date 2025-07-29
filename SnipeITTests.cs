using System;
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
        
        var testAsset = TestData.GenerateAsset();
        await createAssetPage.FillAssetFormAsync(testAsset);
        await createAssetPage.ClickSaveButtonAsync();
        
        // Store the created asset tag for later tests
        createdAssetTag = testAsset.AssetTag;
        
        var isAssetCreated = await createAssetPage.IsAssetCreatedAsync();
        isAssetCreated.Should().BeTrue("Asset should be created successfully");
        
        Console.WriteLine($"Test 3 completed: Asset created with tag {createdAssetTag}");
    }

    [Test, Order(4)]
    public async Task Test4_ViewAssetDetails()
    {
        Console.WriteLine("Starting Test 4: View Asset Details");
        
        createdAssetTag.Should().NotBeNullOrEmpty("Asset tag should be available from previous test");
        
        await assetsPage!.NavigateToAssetsAsync();
        await assetsPage.SearchForAssetAsync(createdAssetTag!);
        await assetsPage.ClickAssetLinkAsync(createdAssetTag!);
        
        var isOnAssetDetailsPage = await assetDetailsPage!.IsOnAssetDetailsPageAsync();
        isOnAssetDetailsPage.Should().BeTrue("Should navigate to asset details page");
        
        var assetTag = await assetDetailsPage.GetAssetTagAsync();
        assetTag.Should().Be(createdAssetTag, "Asset tag should match the created asset");
        
        Console.WriteLine($"Test 4 completed: Successfully viewed asset details for {createdAssetTag}");
    }

    [Test, Order(5)]
    public async Task Test5_DeleteAsset()
    {
        Console.WriteLine("Starting Test 5: Delete Asset");
        
        createdAssetTag.Should().NotBeNullOrEmpty("Asset tag should be available from previous test");
        
        // Ensure we're on the asset details page
        var isOnAssetDetailsPage = await assetDetailsPage!.IsOnAssetDetailsPageAsync();
        if (!isOnAssetDetailsPage)
        {
            await assetsPage!.NavigateToAssetsAsync();
            await assetsPage.SearchForAssetAsync(createdAssetTag!);
            await assetsPage.ClickAssetLinkAsync(createdAssetTag!);
        }
        
        await assetDetailsPage!.ClickDeleteButtonAsync();
        await assetDetailsPage.ConfirmDeletionAsync();
        
        var isAssetDeleted = await assetDetailsPage.IsAssetDeletedAsync();
        isAssetDeleted.Should().BeTrue("Asset should be deleted successfully");
        
        Console.WriteLine($"Test 5 completed: Asset {createdAssetTag} deleted successfully");
    }
}
