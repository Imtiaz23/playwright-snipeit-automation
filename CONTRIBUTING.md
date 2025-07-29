# Contributing to SnipeIT Playwright Automation

Thank you for your interest in contributing to this project! We welcome contributions from the community.

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Git
- Visual Studio 2022, VS Code, or JetBrains Rider
- Basic knowledge of C#, Playwright, and test automation

### Setting Up Development Environment

1. **Fork the repository**
   ```bash
   git clone https://github.com/yourusername/snipeit-playwright-automation.git
   cd snipeit-playwright-automation
   ```

2. **Install dependencies**
   ```bash
   dotnet restore
   ```

3. **Install Playwright browsers**
   ```bash
   cd src/PlaywrightAutomation
   dotnet build
   pwsh bin/Debug/net8.0/playwright.ps1 install chromium
   ```

4. **Run tests to verify setup**
   ```bash
   dotnet test
   ```

## 📋 How to Contribute

### 🐛 Reporting Bugs

1. **Check existing issues** first to avoid duplicates
2. **Use the bug report template** when creating new issues
3. **Provide detailed information**:
   - Steps to reproduce
   - Expected vs actual behavior
   - Environment details (.NET version, OS, etc.)
   - Screenshots or videos if applicable

### 💡 Suggesting Features

1. **Check existing feature requests** first
2. **Use the feature request template**
3. **Explain the use case** and benefits
4. **Provide mockups or examples** if applicable

### 🔧 Code Contributions

#### Before You Start
- **Create an issue** first to discuss your proposed changes
- **Check if someone else is already working** on similar changes
- **Follow the project's coding standards** and patterns

#### Pull Request Process

1. **Create a feature branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make your changes**
   - Follow existing code patterns
   - Add appropriate tests
   - Update documentation if needed

3. **Test your changes**
   ```bash
   dotnet test
   dotnet build
   ```

4. **Commit with descriptive messages**
   ```bash
   git commit -m "Add: New page object for user management"
   ```

5. **Push to your fork**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Create a Pull Request**
   - Use the PR template
   - Reference related issues
   - Describe your changes clearly

## 🎯 Coding Standards

### C# Guidelines
- Follow [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)
- Use **async/await** for all asynchronous operations
- Use **nullable reference types** appropriately
- Use **file-scoped namespaces**

### Test Automation Best Practices
- **Page Object Model**: Keep page logic separate from test logic
- **Explicit waits**: Use Playwright's wait mechanisms, avoid `Thread.Sleep`
- **Meaningful assertions**: Use FluentAssertions with descriptive messages
- **Test data**: Use Bogus for generating realistic test data
- **Clean up**: Ensure proper resource disposal

### Code Examples

#### Good Page Object Method
```csharp
public async Task LoginAsync(string username, string password)
{
    await FillAsync(UsernameInput, username);
    await FillAsync(PasswordInput, password);
    await ClickAsync(LoginButton);
    await WaitForElementAsync(DashboardHeader);
}
```

#### Good Test Method
```csharp
[Test]
[Description("Verify user can create a new asset with valid data")]
public async Task CreateAsset_WithValidData_ShouldSucceed()
{
    // Arrange
    var testAsset = TestData.GenerateAsset();
    
    // Act
    await _loginPage.LoginAsync(TestConfiguration.Credentials.Username, TestConfiguration.Credentials.Password);
    await _assetsPage.NavigateToAssetsAsync();
    await _createAssetPage.CreateAssetAsync(testAsset);
    
    // Assert
    var isVisible = await _assetsPage.IsAssetVisibleInListAsync(testAsset.AssetTag!);
    isVisible.Should().BeTrue("Created asset should be visible in the assets list");
}
```

## 📁 Project Structure

```
src/PlaywrightAutomation/
├── Configuration/     # Test configuration and constants
├── Models/           # Data models (Asset, User, etc.)
├── Pages/            # Page Object Model classes
├── Tests/            # Base test infrastructure
└── Utils/            # Utilities and test data generation

tests/PlaywrightAutomation.Tests/
└── AutomationTests.cs  # Test scenarios
```

### Adding New Page Objects

1. **Inherit from BasePage**
2. **Define selectors as constants**
3. **Use async methods**
4. **Add meaningful method names**

```csharp
public class NewPage : BasePage
{
    private const string SubmitButton = "button[type='submit']";
    private const string SuccessMessage = ".alert-success";
    
    public NewPage(IPage page) : base(page) { }
    
    public async Task SubmitFormAsync()
    {
        await ClickAsync(SubmitButton);
        await WaitForElementAsync(SuccessMessage);
    }
}
```

## 🧪 Testing Guidelines

### Test Categories
- **Unit Tests**: Test individual components
- **Integration Tests**: Test component interactions
- **End-to-End Tests**: Test complete user workflows

### Test Naming Convention
```csharp
[Test]
public async Task MethodName_Condition_ExpectedResult()
{
    // Test implementation
}
```

### Test Structure (AAA Pattern)
```csharp
[Test]
public async Task Example_Test()
{
    // Arrange - Set up test data and preconditions
    var testData = TestData.GenerateAsset();
    
    // Act - Perform the action being tested
    await _page.PerformActionAsync(testData);
    
    // Assert - Verify the expected outcome
    result.Should().BeTrue("Expected condition should be met");
}
```

## 📚 Documentation

### README Updates
- Update the README.md if your changes affect:
  - Installation instructions
  - Configuration options
  - New features or capabilities

### Code Documentation
- Add XML documentation for public methods
- Include examples for complex functionality
- Update inline comments for clarity

```csharp
/// <summary>
/// Creates a new asset with the specified details and validates creation
/// </summary>
/// <param name="asset">The asset details to create</param>
/// <returns>True if asset was created successfully, false otherwise</returns>
public async Task<bool> CreateAssetAsync(Asset asset)
{
    // Implementation
}
```

## 🔍 Code Review Process

### For Contributors
- **Self-review** your code before submitting
- **Test thoroughly** on different environments if possible
- **Respond promptly** to review feedback
- **Keep PRs focused** - one feature/fix per PR

### Review Criteria
- **Functionality**: Does the code work as intended?
- **Tests**: Are there appropriate tests?
- **Performance**: Are there any performance implications?
- **Security**: Are there any security concerns?
- **Maintainability**: Is the code readable and maintainable?

## 🚫 What Not to Contribute

- **Breaking changes** without prior discussion
- **Large refactoring** without approval
- **Dependencies** that haven't been approved
- **Personal configuration** files
- **Credentials** or sensitive information

## 📞 Getting Help

- **GitHub Issues**: For bugs and feature requests
- **GitHub Discussions**: For questions and general discussion
- **Code Comments**: For specific code-related questions

## 🏆 Recognition

Contributors will be:
- **Listed in the README** acknowledgments
- **Mentioned in release notes** for significant contributions
- **Invited as collaborators** for ongoing contributors

Thank you for contributing to making this project better! 🎉
