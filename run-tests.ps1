#!/usr/bin/env pwsh

# SnipeIT Playwright Test Runner
# This script runs the SnipeIT automation tests with comprehensive reporting

Write-Host "Starting SnipeIT Playwright Automation Tests..." -ForegroundColor Green

# Ensure TestResults directory exists
if (!(Test-Path "TestResults")) {
    New-Item -ItemType Directory -Path "TestResults" -Force | Out-Null
    Write-Host "Created TestResults directory" -ForegroundColor Yellow
}

Write-Host "Setting up dependencies..." -ForegroundColor Cyan

# Restore NuGet packages
Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
$restoreResult = dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to restore packages. Please check your .NET installation." -ForegroundColor Red
    exit 1
}

# Build the project to ensure Playwright tools are available
Write-Host "Building project..." -ForegroundColor Yellow
$buildResult = dotnet build --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed. Please check for compilation errors." -ForegroundColor Red
    exit 1
}

# Install Playwright browsers (this is the key step!)
Write-Host "Installing Playwright browsers (this may take a few minutes on first run)..." -ForegroundColor Yellow
$playwrightPath = "bin/Debug/net8.0/playwright.ps1"

if (Test-Path $playwrightPath) {
    try {
        & $playwrightPath install chromium
        if ($LASTEXITCODE -ne 0) {
            throw "Playwright install failed with exit code $LASTEXITCODE"
        }
        Write-Host "Playwright browsers installed successfully!" -ForegroundColor Green
    }
    catch {
        Write-Host "Failed to install Playwright browsers: $_" -ForegroundColor Red
        Write-Host "You may need to run this manually: pwsh bin/Debug/net8.0/playwright.ps1 install chromium" -ForegroundColor Yellow
        exit 1
    }
} else {
    Write-Host "Playwright installation script not found. Please ensure the project built successfully." -ForegroundColor Red
    exit 1
}

# Run tests with comprehensive logging
Write-Host ""
Write-Host "Running tests with comprehensive reporting..." -ForegroundColor Cyan

dotnet test PlaywrightTests.csproj `
    --logger "trx;LogFileName=TestResults.trx" `
    --logger "html;LogFileName=TestResults.html" `
    --verbosity normal `
    --settings test.runsettings

$exitCode = $LASTEXITCODE

# Display results
Write-Host ""
if ($exitCode -eq 0) {
    Write-Host "All tests completed successfully!" -ForegroundColor Green
} else {
    Write-Host "Some tests failed. Check the reports for details." -ForegroundColor Red
}

Write-Host ""
Write-Host "Test Reports Generated:" -ForegroundColor Cyan
Write-Host "   • HTML Report: TestResults/TestResults.html" -ForegroundColor White
Write-Host "   • TRX Report:  TestResults/TestResults.trx" -ForegroundColor White
Write-Host "   • Videos:      bin/Debug/net8.0/videos/" -ForegroundColor White
Write-Host "   • Screenshots: screenshots/" -ForegroundColor White

Write-Host ""
Write-Host "Tip: Open TestResults.html in your browser to view the detailed test report" -ForegroundColor Yellow

exit $exitCode
