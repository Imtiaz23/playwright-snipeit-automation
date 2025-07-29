# Playwright Automation for SnipeIT Demo

This project contains automated tests for the SnipeIT demo application using Playwright with .NET 8 and NUnit.

## Project Structure

```
playwright-automation/
├── src/
│   └── PlaywrightAutomation/
│       ├── Configuration/
│       │   └── TestConfiguration.cs      # Test configuration and constants
│       ├── Models/
│       │   ├── Asset.cs                  # Asset model
│       │   └── User.cs                   # User model
│       ├── Pages/
│       │   ├── BasePage.cs               # Base page with common functionality
│       │   ├── LoginPage.cs              # Login page methods
│       │   ├── AssetsPage.cs             # Assets list page methods
│       │   ├── CreateAssetPage.cs        # Asset creation page methods
│       │   └── AssetDetailsPage.cs       # Asset details page methods
│       ├── Tests/
│       │   └── BaseTest.cs               # Base test class with setup/teardown
│       ├── Utils/
│       │   └── TestData.cs               # Test data generation using Bogus
│       └── PlaywrightAutomation.csproj
├── tests/
│   └── PlaywrightAutomation.Tests/
│       ├── AutomationTests.cs            # Main test scenarios
│       └── PlaywrightAutomation.Tests.csproj
├── playwright.config.json               # Playwright configuration
├── test.runsettings                      # NUnit test settings
└── README.md
```

## Features

- **Modern .NET 8** with latest Playwright version (1.47.0)
- **Page Object Model** design pattern for maintainable test code
- **Fluent Assertions** for readable test assertions
- **Bogus** library for realistic test data generation
- **Comprehensive logging** and error handling
- **Screenshot capture** on test failures
- **Video recording** for test sessions
- **NUnit** test framework with detailed reporting

## Test Scenarios

### Main Test: `CreateAndVerifyMacBookProAsset`

This comprehensive test performs the following workflow:

1. **Login** to SnipeIT demo (https://demo.snipeitapp.com/login)
2. **Create** a new MacBook Pro 13" asset with:
   - Ready to Deploy status
   - Random asset tag and serial number
   - Checked out to a random user
3. **Search** for the created asset in the assets list
4. **Verify** the asset appears in the search results
5. **Navigate** to the asset details page
6. **Validate** all asset details match the created asset
7. **Check History tab** for asset creation/checkout entries

### Additional Tests

- `VerifyLoginFunctionality` - Tests login with valid credentials
- `VerifyAssetsPageLoads` - Verifies assets page loads and displays data

## Prerequisites

- .NET 8 SDK
- Visual Studio 2022 or VS Code
- Playwright browsers (automatically installed)

## Setup and Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd playwright-automation
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Install Playwright browsers**
   ```bash
   dotnet exec pwsh src/PlaywrightAutomation/bin/Debug/net8.0/playwright.ps1 install
   ```
   Or manually:
   ```bash
   playwright install
   ```

## Running Tests

### Command Line

Run all tests:
```bash
dotnet test
```

Run with detailed output:
```bash
dotnet test --verbosity detailed
```

Run specific test:
```bash
dotnet test --filter "CreateAndVerifyMacBookProAsset"
```

Run with custom settings:
```bash
dotnet test --settings test.runsettings
```

### Visual Studio

1. Open `src/PlaywrightAutomation.sln`
2. Build the solution
3. Open Test Explorer
4. Run tests from the Test Explorer

### VS Code

1. Install C# extension
2. Install .NET Test Explorer extension
3. Open the project folder
4. Use Test Explorer to run tests

## Configuration

### Test Configuration

Update `src/PlaywrightAutomation/Configuration/TestConfiguration.cs` to modify:
- Base URLs
- Default credentials
- Timeouts
- Asset defaults

### Playwright Configuration

Update `playwright.config.json` to modify:
- Browser settings
- Viewport size
- Video/screenshot settings
- Retry policies

## Test Data

The project uses the **Bogus** library to generate realistic test data:

- Random asset tags (8 alphanumeric characters)
- Random serial numbers (12 alphanumeric characters)
- Random user names and details
- Lorem ipsum text for notes

## Reporting

The project generates multiple types of reports:

- **HTML Report**: `playwright-report/index.html`
- **JSON Results**: `test-results.json`
- **JUnit XML**: `test-results.xml`
- **Screenshots**: `screenshots/` directory (on failure)
- **Videos**: `videos/` directory (on failure)

## Best Practices Implemented

1. **Page Object Model**: Clean separation of page logic and test logic
2. **Async/Await**: Proper async handling throughout
3. **Explicit Waits**: No hard-coded delays, using Playwright's wait mechanisms
4. **Error Handling**: Comprehensive error messages and logging
5. **Test Data Management**: Isolated test data generation
6. **Resource Cleanup**: Proper browser cleanup in teardown
7. **Readable Assertions**: Fluent assertions with descriptive messages

## Troubleshooting

### Common Issues

1. **Browser not installed**
   ```bash
   playwright install chromium
   ```

2. **Compilation errors**
   ```bash
   dotnet clean
   dotnet restore
   dotnet build
   ```

3. **Test timeouts**
   - Increase timeouts in `TestConfiguration.cs`
   - Check network connectivity to demo site

### Debug Mode

Run tests with browser visible:
- Set `Headless = false` in `BaseTest.cs`
- Or modify `playwright.config.json`

### Logs and Screenshots

Check the following directories after test runs:
- `screenshots/` - Screenshots on failure
- `videos/` - Video recordings
- `test-results/` - Test output files

## Demo Credentials

The tests use the default SnipeIT demo credentials:
- **Username**: admin
- **Password**: password
- **URL**: https://demo.snipeitapp.com

## Contributing

1. Follow the existing code structure and patterns
2. Add appropriate error handling and logging
3. Update tests when adding new functionality
4. Ensure all tests pass before submitting changes

## License

This project is for demonstration and educational purposes.
