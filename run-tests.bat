@echo off
setlocal enabledelayedexpansion

echo.
echo 🚀 Starting SnipeIT Playwright Automation Tests...
echo.

REM Ensure TestResults directory exists
if not exist "TestResults" (
    mkdir "TestResults"
    echo 📁 Created TestResults directory
)

echo 🧪 Running tests with comprehensive reporting...
echo.

REM Run tests with comprehensive logging
dotnet test PlaywrightTests.csproj --logger "trx;LogFileName=TestResults.trx" --logger "html;LogFileName=TestResults.html" --verbosity normal --settings test.runsettings

set TEST_EXIT_CODE=!ERRORLEVEL!

echo.
if !TEST_EXIT_CODE! equ 0 (
    echo ✅ All tests completed successfully!
) else (
    echo ❌ Some tests failed. Check the reports for details.
)

echo.
echo 📊 Test Reports Generated:
echo    • HTML Report: TestResults\TestResults.html
echo    • TRX Report:  TestResults\TestResults.trx
echo    • Videos:      bin\Debug\net8.0\videos\
echo    • Screenshots: screenshots\

echo.
echo 💡 Tip: Open TestResults.html in your browser to view the detailed test report
echo.

exit /b !TEST_EXIT_CODE!
