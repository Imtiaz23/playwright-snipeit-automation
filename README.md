# Playwright Automation Project

## Overview
This project is designed to automate web application testing using Playwright. It provides a structured approach to organizing page objects, tests, and utility functions to facilitate efficient test automation.

# 🎭 SnipeIT Playwright Automation

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Playwright](https://img.shields.io/badge/Playwright-1.47.0-green.svg)](https://playwright.dev/)
[![NUnit](https://img.shields.io/badge/NUnit-4.2.2-blue.svg)](https://nunit.org/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A comprehensive .NET 8 Playwright automation project that tests the [SnipeIT demo application](https://demo.snipeitapp.com) using modern C# practices and industry-standard test automation patterns.

## 🚀 Features

- **Modern .NET 8** with latest C# language features
- **Playwright 1.47.0** with cutting-edge browser automation
- **Page Object Model** design pattern for maintainable tests
- **Realistic test data** generation with Bogus library
- **Comprehensive reporting** with screenshots and videos
- **Fluent assertions** for readable test validation
- **Cross-platform support** (Windows, macOS, Linux)

## 📋 Test Scenarios

### 🎯 Main Workflow: Asset Creation & Verification
The comprehensive test performs a complete end-to-end workflow:

1. **🔐 Login** to SnipeIT demo with admin credentials
2. **📱 Create MacBook Pro 13"** asset with:
   - ✅ Ready to Deploy status
   - 🏷️ Random asset tag and serial number
   - 👤 Checkout to random user
3. **🔍 Search & Verify** asset appears in assets list
4. **📄 Navigate** to asset details page
5. **✔️ Validate** all asset details match created data
6. **📈 Check History tab** for creation/checkout entries

### 🧪 Additional Tests
- **Login functionality** validation
- **Assets page** loading verification
- **Error handling** and edge cases

## 🏗️ Architecture

```
📦 playwright-automation/
├── 📁 src/PlaywrightAutomation/          # Core automation library
│   ├── 📁 Configuration/                 # Test settings & constants
│   ├── 📁 Models/                        # Data models (Asset, User)
│   ├── 📁 Pages/                         # Page Object Model classes
│   │   ├── 📄 BasePage.cs               # Common page functionality
│   │   ├── 📄 LoginPage.cs              # Login interactions
│   │   ├── 📄 AssetsPage.cs             # Assets list management
│   │   ├── 📄 CreateAssetPage.cs        # Asset creation workflow
│   │   └── 📄 AssetDetailsPage.cs       # Asset details & history
│   ├── 📁 Tests/                         # Base test infrastructure
│   ├── 📁 Utils/                         # Test utilities & data generation
│   └── 📄 PlaywrightAutomation.csproj   # Main project file
├── 📁 tests/PlaywrightAutomation.Tests/  # Test implementation
│   ├── 📄 AutomationTests.cs            # Test scenarios
│   └── 📄 PlaywrightAutomation.Tests.csproj
├── 📄 playwright.config.json            # Playwright configuration
├── 📄 test.runsettings                   # NUnit test settings
└── 📄 README.md                          # This file
```

## 🛠️ Prerequisites

- **[.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)** - Latest LTS version
- **[Git](https://git-scm.com/)** - For version control
- **IDE**: Visual Studio 2022, VS Code, or JetBrains Rider
- **Internet connection** - For SnipeIT demo access

## 🚀 Quick Start

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/snipeit-playwright-automation.git
cd snipeit-playwright-automation
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Install Playwright Browsers
```bash
# Navigate to the main project
cd src/PlaywrightAutomation

# Build the project first
dotnet build

# Install Playwright browsers
pwsh bin/Debug/net8.0/playwright.ps1 install chromium
```

### 4. Run the Tests
```bash
# From the root directory
dotnet test

# With detailed output
dotnet test --verbosity detailed

# Run specific test
dotnet test --filter "CreateAndVerifyMacBookProAsset"
```

## 🎬 Running Tests

### Command Line Options

```bash
# Run all tests
dotnet test

# Run with detailed logging
dotnet test --verbosity detailed

# Run specific test method
dotnet test --filter "MethodName~CreateAndVerify"

# Run with custom settings
dotnet test --settings test.runsettings

# Generate coverage report
dotnet test --collect:"XPlat Code Coverage"
```

### 🖥️ Visible Browser Mode
Tests are configured to run with visible browser (non-headless) with 1-second delays between actions for easy observation.

### 📊 Test Reports
- **Console Output**: Real-time test progress
- **Screenshots**: `screenshots/` directory (on failure)
- **Videos**: `videos/` directory (for test sessions)
- **HTML Reports**: Generated in `playwright-report/`

## 🔧 Configuration

### Test Settings (`TestConfiguration.cs`)
```csharp
public static class TestConfiguration
{
    public static class Urls
    {
        public const string SnipeItLogin = "https://demo.snipeitapp.com/login";
        public const string SnipeItBase = "https://demo.snipeitapp.com";
    }
    
    public static class Credentials
    {
        public const string Username = "admin";
        public const string Password = "password";
    }
    
    // Timeouts, defaults, etc.
}
```

### Playwright Settings (`playwright.config.json`)
- **Browser**: Chromium (latest)
- **Headless**: False (visible browser)
- **SlowMo**: 1000ms (1-second delays)
- **Viewport**: 1920x1080
- **Video Recording**: On failure
- **Screenshots**: On failure

## 📊 Technology Stack

| Component | Version | Purpose |
|-----------|---------|---------|
| .NET | 8.0 | Runtime framework |
| Playwright | 1.47.0 | Browser automation |
| NUnit | 4.2.2 | Test framework |
| FluentAssertions | 6.12.1 | Readable assertions |
| Bogus | 35.6.1 | Test data generation |

## 🎯 Key Features

### 🧩 Page Object Model
Clean separation of concerns with dedicated page classes:
```csharp
public class LoginPage : BasePage
{
    public async Task LoginAsync(string username, string password)
    {
        await FillAsync(UsernameInput, username);
        await FillAsync(PasswordInput, password);
        await ClickAsync(LoginButton);
    }
}
```

### 🎲 Realistic Test Data
Generate dynamic test data with Bogus:
```csharp
private static readonly Faker<Asset> AssetFaker = new Faker<Asset>()
    .RuleFor(a => a.AssetTag, f => f.Random.AlphaNumeric(8).ToUpper())
    .RuleFor(a => a.SerialNumber, f => f.Random.AlphaNumeric(12).ToUpper())
    .RuleFor(a => a.Model, "MacBook Pro 13"");
```

### 🎬 Comprehensive Logging
```csharp
Console.WriteLine($"Creating asset with tag: {testAsset.AssetTag}");
Console.WriteLine($"Asset {testAsset.AssetTag} found in assets list ✓");
Console.WriteLine("=== Test Completed Successfully ===");
```

## 🐛 Debugging

### Screenshots & Videos
- **Automatic screenshots** on test failures
- **Video recordings** of entire test sessions
- **Console logging** throughout test execution

### Debug Mode
```csharp
// In BaseTest.cs - adjust SlowMo for slower execution
Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions 
{ 
    Headless = false,    // Visible browser
    SlowMo = 2000       // 2-second delays
});
```

## 🤝 Contributing

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/amazing-feature`)
3. **Commit** your changes (`git commit -m 'Add amazing feature'`)
4. **Push** to the branch (`git push origin feature/amazing-feature`)
5. **Open** a Pull Request

## 📝 Best Practices

### ✅ Code Quality
- **SOLID principles** throughout
- **Async/await** patterns
- **Explicit waits** instead of hard delays
- **Meaningful error messages**
- **Proper resource cleanup**

### ✅ Test Quality
- **Descriptive test names**
- **Clear test documentation**
- **Isolated test data**
- **Comprehensive assertions**

## 📋 Roadmap

- [ ] **API Testing** integration
- [ ] **Cross-browser** testing (Firefox, Safari)
- [ ] **Mobile testing** with device emulation
- [ ] **CI/CD pipeline** configuration
- [ ] **Performance testing** scenarios
- [ ] **Accessibility testing** integration

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/yourusername/snipeit-playwright-automation/issues)
- **Discussions**: [GitHub Discussions](https://github.com/yourusername/snipeit-playwright-automation/discussions)
- **Documentation**: [Wiki](https://github.com/yourusername/snipeit-playwright-automation/wiki)

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **[SnipeIT](https://snipeitapp.com/)** - Open source asset management
- **[Playwright](https://playwright.dev/)** - Modern web testing framework
- **[Microsoft .NET](https://dotnet.microsoft.com/)** - Development platform
- **[NUnit](https://nunit.org/)** - Testing framework

---

**⭐ Star this repository if you find it helpful!**

## Setup Instructions
1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd playwright-automation
   ```

2. **Install Dependencies**
   Ensure you have the latest version of .NET SDK installed. Then, run the following command to restore the project dependencies:
   ```bash
   dotnet restore
   ```

3. **Run the Tests**
   To execute the tests, use the following command:
   ```bash
   dotnet test tests/PlaywrightAutomation.Tests/PlaywrightAutomation.Tests.csproj
   ```

## Usage
The project includes various components:
- **Page Objects**: Located in the `Pages` directory, these classes encapsulate the interactions with the web pages.
- **Tests**: The `Tests` directory contains test classes that implement the automation steps.
- **Utilities**: The `Utils` directory provides helper methods for generating test data.

## Contributing
Contributions are welcome! Please submit a pull request or open an issue for any enhancements or bug fixes.

## License
This project is licensed under the MIT License. See the LICENSE file for more details.