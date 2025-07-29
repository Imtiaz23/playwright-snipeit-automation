using Bogus;
using PlaywrightAutomation.Configuration;
using PlaywrightAutomation.Models;

namespace PlaywrightAutomation.Utils;

public static class TestData
{
    private static readonly Random random = new();

    private static readonly Faker<Asset> AssetFaker = new Faker<Asset>()
        .RuleFor(a => a.AssetTag, f => f.Random.AlphaNumeric(8).ToUpper())
        .RuleFor(a => a.Model, TestConfiguration.AssetDefaults.Model)
        .RuleFor(a => a.Manufacturer, TestConfiguration.AssetDefaults.Manufacturer)
        .RuleFor(a => a.Category, TestConfiguration.AssetDefaults.Category)
        .RuleFor(a => a.Status, TestConfiguration.AssetDefaults.Status)
        .RuleFor(a => a.SerialNumber, f => f.Random.AlphaNumeric(12).ToUpper())
        .RuleFor(a => a.Notes, f => f.Lorem.Sentence(10));

    private static readonly Faker<User> UserFaker = new Faker<User>()
        .RuleFor(u => u.FirstName, f => f.Name.FirstName())
        .RuleFor(u => u.LastName, f => f.Name.LastName())
        .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
        .RuleFor(u => u.Username, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
        .RuleFor(u => u.EmployeeNumber, f => f.Random.Number(1000, 9999).ToString());

    public static Asset GenerateAsset() => AssetFaker.Generate();

    public static User GenerateUser() => UserFaker.Generate();

    public static List<Asset> GenerateAssets(int count) => AssetFaker.Generate(count);

    public static List<User> GenerateUsers(int count) => UserFaker.Generate(count);

    // Legacy methods for backward compatibility
    public static string GenerateRandomUsername() => "user" + random.Next(1000, 9999);

    public static string GenerateRandomEmail() => "user" + random.Next(1000, 9999) + "@example.com";

    public static string GenerateRandomPassword() => "Password" + random.Next(1000, 9999);

    public static string GenerateRandomAssetName() => "Asset" + random.Next(1000, 9999);

    public static string GenerateRandomAssetDescription() => "Description for asset " + random.Next(1000, 9999);
}