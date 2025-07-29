using System;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
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

    [OneTimeSetUp]
    public async Task ClassSetUpAsync()
    {
        await base.OneTimeSetUpAsync();
        
        loginPage = new LoginPage(Page!);
        assetsPage = new AssetsPage(Page!);
        createAssetPage = new CreateAssetPage(Page!);
        assetDetailsPage = new AssetDetailsPage(Page!);
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
    public async Task Test2_NavigateToAssets()
    {
        Console.WriteLine("Starting Test 2: Navigate to Assets");
        
        await assetsPage!.NavigateToAssetsAsync();
        
        var isOnAssetsPage = await assetsPage.IsOnAssetsPageAsync();
        isOnAssetsPage.Should().BeTrue("Should successfully navigate to assets page");
        
        Console.WriteLine("Test 2 completed: Successfully navigated to Assets page");
    }

    [Test, Order(3)]
    public async Task Test3_CreateAsset()
    {
        Console.WriteLine("Starting Test 3: Create Asset");
        
        await assetsPage!.ClickCreateAssetAsync();
        
        var isOnCreateAssetPage = await createAssetPage!.IsOnCreateAssetPageAsync();
        isOnCreateAssetPage.Should().BeTrue("Should navigate to create asset page");
        
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
