#!/usr/bin/env pwsh

# SnipeIT Playwright Test Runner
# This script runs the SnipeIT automation tests with comprehensive reporting

Write-Host "🚀 Starting SnipeIT Playwright Automation Tests..." -ForegroundColor Green

# Ensure TestResults directory exists
if (!(Test-Path "TestResults")) {
    New-Item -ItemType Directory -Path "TestResults" -Force | Out-Null
    Write-Host "📁 Created TestResults directory" -ForegroundColor Yellow
}

# Run tests with comprehensive logging
Write-Host "🧪 Running tests with comprehensive reporting..." -ForegroundColor Cyan

dotnet test PlaywrightTests.csproj `
    --logger "trx;LogFileName=TestResults.trx" `
    --logger "html;LogFileName=TestResults.html" `
    --verbosity normal `
    --settings test.runsettings

$exitCode = $LASTEXITCODE

# Display results
Write-Host ""
if ($exitCode -eq 0) {
    Write-Host "✅ All tests completed successfully!" -ForegroundColor Green
} else {
    Write-Host "❌ Some tests failed. Check the reports for details." -ForegroundColor Red
}

Write-Host ""
Write-Host "📊 Test Reports Generated:" -ForegroundColor Cyan
Write-Host "   • HTML Report: TestResults/TestResults.html" -ForegroundColor White
Write-Host "   • TRX Report:  TestResults/TestResults.trx" -ForegroundColor White
Write-Host "   • Videos:      bin/Debug/net8.0/videos/" -ForegroundColor White
Write-Host "   • Screenshots: screenshots/" -ForegroundColor White

Write-Host ""
Write-Host "💡 Tip: Open TestResults.html in your browser to view the detailed test report" -ForegroundColor Yellow

exit $exitCode
