using Bogus;
using PlaywrightAutomation.Configuration;
using PlaywrightAutomation.Models;

namespace PlaywrightAutomation.Utils;

public static class TestData
{
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
        .RuleFor(u => u.Department, f => f.Commerce.Department())
        .RuleFor(u => u.Location, f => f.Address.City());

    public static Asset GenerateAsset() => AssetFaker.Generate();
    public static User GenerateUser() => UserFaker.Generate();
}
