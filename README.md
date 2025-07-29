# SnipeIT Playwright Automation

[![## 🛠 Setup InstructionsNET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Playwright](https://img.shields.io/badge/Playwright-1.47.0-green.svg)](https://playwright.dev/)
[![NUnit](https://img.shields.io/badge/NUnit-4.2.2-blue.svg)](https://nunit.org/)

A comprehensive .NET 8 Playwright automation project that tests the [SnipeIT demo application](https://develop.snipeitapp.com/) using modern C# practices and industry-standard test automation patterns.

## 🚀 Features

- **Sequential Test Execution**: Tests run in order without requiring repeated logins
- **Video Recording**: All test executions are recorded for debugging
- **Test Reports**: Generates both TRX and HTML test reports
- **Page Object Model**: Clean, maintainable code structure
- **Cross-browser Support**: Built on Playwright for reliable browser automation

## 📋 Prerequisites

Before running the tests, ensure you have the following installed:

### 1. .NET 8.0 SDK
Download and install from [Microsoft .NET Downloads](https://dotnet.microsoft.com/download/dotnet/8.0)

Verify installation:
```bash
dotnet --version
```

### 2. Playwright Browsers
Playwright browsers will be installed automatically when you first run the tests.

## � Setup Instructions

### 1. Clone the Repository
```bash
git clone https://github.com/Imtiaz23/playwright-snipeit-automation.git
cd playwright-snipeit-automation
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Install Playwright Browsers (Optional)
```bash
# Browsers will install automatically, but you can pre-install them:
dotnet build
pwsh bin/Debug/net8.0/playwright.ps1 install chromium
```

## 🧪 Running Tests

### Run Tests
```bash
dotnet test PlaywrightTests.csproj --logger "trx;LogFileName=TestResults.trx" --logger "html;LogFileName=TestResults.html" --verbosity normal
```

### Test Cases
1. **Test1_Login**: Authenticates user and verifies successful login
2. **Test2_NavigateToCreateAsset**: Navigates to asset creation page via dropdown menu
3. **Test3_CreateAsset**: Creates a new asset with generated test data
4. **Test4_ViewAssetDetails**: Searches for and views the created asset
5. **Test5_DeleteAsset**: Deletes the created asset

## 📊 Test Reports and Artifacts

After running tests, you'll find the following artifacts:

### Test Reports
- **HTML Report**: `TestResults/TestResults.html` - Human-readable test results
- **TRX Report**: `TestResults/TestResults.trx` - Machine-readable test results

### Video Recordings
- Location: `bin/Debug/net8.0/videos/`
- Format: WebM files for each test execution
- Resolution: 1920x1080

### Screenshots
- Location: `bin/Debug/net8.0/screenshots/`
- Automatically captured on test failures
- Format: PNG files with timestamp

## 🔧 Configuration

### Test Configuration
Edit `Configuration/TestConfiguration.cs` to modify:
- Base URL
- Login credentials
- Timeouts

### Playwright Configuration
Edit `playwright.config.json` to modify:
- Browser settings
- Video recording options
- Screenshot settings

## 📁 Project Structure

```
playwright-automation/
├── Configuration/          # Test configuration settings
├── Models/                 # Data models for test objects
├── Pages/                  # Page Object Model classes
│   ├── LoginPage.cs
│   ├── AssetsPage.cs
│   ├── CreateAssetPage.cs
│   └── AssetDetailsPage.cs
├── Utils/                  # Utility classes and test data generation
├── TestResults/            # Generated test reports
├── BaseTest.cs             # Base test class with setup/teardown
├── SnipeITTests.cs         # Main test class
└── PlaywrightTests.csproj  # Project file
```

**Happy Testing!** 🚀

### 🖥️ Visible Browser Mode
Tests are configured to run with visible browser (non-headless) with 1-second delays between actions for easy observation.