# SnipeIT Playwright Automation
## 🚀 Quick Start (One Command Setup)

### 1. Clone the Repository
```bash
git clone https://github.com/Imtiaz23/playwright-snipeit-automation.git
cd playwright-snipeit-automation
```

### 2. Run Tests (Everything Automated!)
**Windows users can simply run:**
```bash
# PowerShell (Recommended)
.\run-tests.ps1

# Command Prompt
run-tests.bat
```

**That's it!** The scripts will automatically:
- ✅ Restore NuGet packages (`dotnet restore`)
- ✅ Build the project (`dotnet build`)  
- ✅ Install Playwright browsers (first run only)
- ✅ Run all tests with comprehensive reporting
- ✅ Generate HTML and TRX reports

### Manual Setup (Optional)
If you prefer manual control:

```bash
# Step 1: Restore dependencies
dotnet restore

# Step 2: Build project
dotnet build

# Step 3: Install Playwright browsers (one-time setup)
pwsh bin/Debug/net8.0/playwright.ps1 install chromium

# Step 4: Run tests
dotnet test PlaywrightTests.csproj --logger "trx;LogFileName=TestResults.trx" --logger "html;LogFileName=TestResults.html" --verbosity normal --settings test.runsettings


A comprehensive .NET 8 Playwright automation project that tests the [SnipeIT demo application](https://develop.snipeitapp.com/) using modern C# practices and industry-standard test automation patterns.

**🎯 Quick Start**: Just run `.\run-tests.ps1` and everything is automated!

## 🚀 Features

- **One-Command Setup**: Fully automated installation and execution
- **Sequential Test Execution**: Tests run in order without requiring repeated logins
- **Video Recording**: All test executions are recorded for debugging
- **Test Reports**: Generates both TRX and HTML test reports
- **Page Object Model**: Clean, maintainable code structure
- **Cross-browser Support**: Built on Playwright for reliable browser automation
- **Zero Manual Setup**: Scripts handle all dependencies automatically

## 📋 Prerequisites

Before running the tests, ensure you have the following installed:

### 1. .NET 8.0 SDK
Download and install from [Microsoft .NET Downloads](https://dotnet.microsoft.com/download/dotnet/8.0)

Verify installation:
```bash
dotnet --version
```

### 2. PowerShell (Windows users)
- **Windows 10/11**: Already included
- **Older Windows**: Download PowerShell 7+ from [Microsoft PowerShell](https://github.com/PowerShell/PowerShell)

**Note**: Playwright browsers will be installed automatically when you first run the tests.

## � Setup Instructions

### 1. Clone the Repository
```bash
git clone https://github.com/Imtiaz23/playwright-snipeit-automation.git (or #download zip and extract)
<span style="color: blue;">### 2. Navigate to directory with powershell</span>
cd playwright-snipeit-automation-main
```

## 🧪 Running Tests

### Quick Start (Recommended)
Use the provided scripts for the best experience:

**Windows PowerShell**
```bash
# PowerShell (Recommended - handles everything automatically)
.\run-tests.ps1

# or
.\run-tests.bat
```

**Cross-platform (Manual):**
```bash
# Step 1: Restore and build
dotnet restore
dotnet build

# Step 2: Install Playwright browsers (REQUIRED - one-time setup)
pwsh bin/Debug/net8.0/playwright.ps1 install chromium

# Step 3: Run tests
dotnet test PlaywrightTests.csproj --logger "trx;LogFileName=TestResults.trx" --logger "html;LogFileName=TestResults.html" --verbosity normal --settings test.runsettings
```

**Note**: The scripts handle all setup automatically including NuGet restore, build, and Playwright browser installation.

⚠️ **Important**: If using the manual cross-platform commands, you MUST install Playwright browsers first, or tests will fail.

### VS Code Integration
- Press `Ctrl+Shift+P` → "Tasks: Run Task" → "Run SnipeIT Tests"
- Or use the Terminal → Run Task menu

### Test Cases
1. **Test1_Login**: Authenticates user and verifies successful login
2. **Test2_NavigateToCreateAsset**: Navigates to asset creation page via dropdown menu
3. **Test3_CreateAsset**: Creates a new asset with generated test data
4. **Test4_ViewAssetDetails**: Verifies asset appears in recent activity dashboard
5. **Test5_SearchAndVerifyAsset**: Searches for asset and verifies details in assets page
6. **Test6_VerifyAssetDetailsPage**: Validates asset details page elements
7. **Test7_VerifyAssetHistory**: Checks asset history with 2 expected entries

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
- Location: `screenshots/` (root directory)
- Automatically captured on test failures
- Format: PNG files with timestamp

## 🔧 Configuration

### Test Configuration
Edit `Configuration/TestConfiguration.cs` to modify:
- Base URL
- Login credentials
- Timeouts
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
├── TestResults/            # Generated test reports
├── screenshots/            # Failure screenshots
├── run-tests.ps1          # Automated PowerShell runner
├── run-tests.bat          # Automated batch runner
├── BaseTest.cs            # Base test class with setup/teardown
├── SnipeITTests.cs        # Main test class with 7 tests
└── PlaywrightTests.csproj # Project file
```

**Happy Testing!** 🚀

### 🖥️ Visible Browser Mode
Tests are configured to run with visible browser (non-headless) with 1-second delays between actions for easy observation.