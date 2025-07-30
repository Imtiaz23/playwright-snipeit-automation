@echo off
setlocal enabledelayedexpansion

echo.
echo Starting SnipeIT Playwright Automation Tests...
echo.

REM Ensure TestResults directory exists
if not exist "TestResults" (
    mkdir "TestResults"
    echo Created TestResults directory
)

echo Setting up dependencies...
echo.

REM Restore NuGet packages
echo Restoring NuGet packages...
dotnet restore
if !ERRORLEVEL! neq 0 (
    echo Failed to restore packages. Please check your .NET installation.
    pause
    exit /b 1
)

REM Build the project to ensure Playwright tools are available
echo Building project...
dotnet build --no-restore
if !ERRORLEVEL! neq 0 (
    echo Build failed. Please check for compilation errors.
    pause
    exit /b 1
)

REM Install Playwright browsers (this is the key step!)
echo Installing Playwright browsers (this may take a few minutes on first run)...
pwsh bin\Debug\net8.0\playwright.ps1 install chromium
if !ERRORLEVEL! neq 0 (
    echo Failed to install Playwright browsers. Trying alternative method...
    dotnet exec bin\Debug\net8.0\playwright.ps1 install chromium
    if !ERRORLEVEL! neq 0 (
        echo Could not install Playwright browsers. Please install PowerShell or run: dotnet tool install --global Microsoft.Playwright.CLI
        pause
        exit /b 1
    )
)

echo.
echo Running tests with comprehensive reporting...
echo.

REM Run tests with comprehensive logging
dotnet test PlaywrightTests.csproj --logger "trx;LogFileName=TestResults.trx" --logger "html;LogFileName=TestResults.html" --verbosity normal --settings test.runsettings

set TEST_EXIT_CODE=!ERRORLEVEL!

echo.
if !TEST_EXIT_CODE! equ 0 (
    echo All tests completed successfully!
) else (
    echo Some tests failed. Check the reports for details.
)

echo.
echo Test Reports Generated:
echo    HTML Report: TestResults\TestResults.html
echo    TRX Report:  TestResults\TestResults.trx
echo    Videos:      bin\Debug\net8.0\videos\
echo    Screenshots: screenshots\

echo.
echo Tip: Open TestResults.html in your browser to view the detailed test report
echo.

exit /b !TEST_EXIT_CODE!
