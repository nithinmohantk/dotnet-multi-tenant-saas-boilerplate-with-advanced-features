# Contributing to Multi-Tenant SaaS Boilerplate

Thank you for your interest in contributing to our Multi-Tenant SaaS Boilerplate! This document provides guidelines and standards for contributors to ensure a smooth and effective collaboration process.

## 🤝 Welcome Contributors!

We welcome all types of contributions, including but not limited to:
- Bug fixes and improvements
- New features and enhancements
- Documentation improvements
- Test coverage improvements
- Performance optimizations
- Security improvements

> **⚠️ Important**: This is currently a legacy .NET 8.0 project requiring upgrade to .NET 10. Please ensure all contributions are compatible with .NET 10 and tested with Visual Studio 2026.

## 🚀 Getting Started

### Prerequisites

- **.NET 10 SDK** (when upgrading from legacy .NET 8.0)
- **Visual Studio 2026** for development and testing
- **Docker Desktop** for containerized development
- **Azure CLI** for Azure resource management
- **Git** for version control

### Development Setup

1. **Fork the Repository**
   ```bash
   # Fork the repository on GitHub, then clone your fork
   git clone https://github.com/your-username/dotnet-multi-tenant-saas-boilerplate-with-advanced-features.git
   cd dotnet-multi-tenant-saas-boilerplate-with-advanced-features
   ```

2. **Add Upstream Remote**
   ```bash
   git remote add upstream https://github.com/original-owner/dotnet-multi-tenant-saas-boilerplate-with-advanced-features.git
   ```

3. **Create a Feature Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

4. **Set Up Development Environment**
   ```bash
   # Restore dependencies
   dotnet restore
   
   # Build the solution
   dotnet build
   
   # Run tests
   dotnet test
   
   # Start local development (optional)
   docker-compose up -d
   dotnet run --project src/SaasBoilerplate.WebApi
   ```

## 📋 Contribution Types

### 🐛 Bug Reports

Before creating a bug report, please check existing issues to avoid duplicates.

When reporting a bug, include:
- **Clear description** of the issue
- **Steps to reproduce** the problem
- **Expected vs. actual behavior**
- **Environment details** (OS, .NET version, etc.)
- **Relevant logs** or error messages
- **Screenshots** if applicable

### ✨ Feature Requests

For new feature suggestions:
- **Use case**: Describe the problem you're trying to solve
- **Proposed solution**: How you envision the feature working
- **Alternatives considered**: Other approaches you've thought about
- **Additional context**: Any relevant information

### 🔧 Code Contributions

#### Code Standards

1. **C# Coding Standards**
   - Follow [Microsoft C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
   - Use meaningful variable and method names
   - Keep methods and classes small and focused
   - Add XML documentation for public APIs

2. **Clean Architecture Principles**
   - Respect layer boundaries (Domain → Application → Infrastructure → API)
   - Dependencies should point inward
   - Use dependency injection properly
   - Implement proper separation of concerns

3. **Multi-Tenancy Considerations**
   - Always implement `IMustHaveTenant` for tenant-specific entities
   - Use tenant-aware services and repositories
   - Ensure proper data isolation
   - Test with multiple tenants

#### Testing Requirements

All contributions must include appropriate tests:

1. **Unit Tests**
   - Test business logic in isolation
   - Use Moq for mocking dependencies
   - Achieve minimum 80% code coverage
   - Test edge cases and error conditions

2. **Integration Tests**
   - Test API endpoints
   - Test database operations
   - Test middleware functionality
   - Test tenant isolation

3. **BDD Tests (SpecFlow)**
   - Define behavior scenarios
   - Test user workflows
   - Document business requirements

#### Pull Request Process

1. **Before Submitting**
   - [ ] Ensure code builds without errors
   - [ ] All tests pass
   - [ ] Code follows project standards
   - [ ] Documentation is updated
   - [ ] Commit messages are clear

2. **Pull Request Requirements**
   - **Clear title** describing the change
   - **Detailed description** of what was changed and why
   - **Linked issues** (fixes #123)
   - **Testing instructions** if applicable
   - **Screenshots** for UI changes
   - **Breaking changes** clearly documented

## 🔄 Branching Strategy

### Branch Naming Conventions

- `feature/feature-name` - New features
- `fix/bug-description` - Bug fixes
- `hotfix/critical-fix` - Critical production fixes
- `docs/documentation-update` - Documentation changes
- `refactor/code-cleanup` - Code refactoring

### Git Workflow

We use [GitHub Flow](https://guides.github.com/introduction/flow/index.html), so all code changes happen through pull requests:

1. **Main Branch**: `main` - Always stable and deployable
2. **Feature Branches**: Created from `main`, merged back to `main`
3. **Pull Requests**: Required for all changes

## 📝 Commit Message Standards

Use [Conventional Commits](https://www.conventionalcommits.org/) format:

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

### Types
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting, etc.)
- `refactor`: Code refactoring
- `test`: Adding or updating tests
- `chore`: Maintenance tasks
- `perf`: Performance improvements
- `security`: Security improvements

### Examples
```
feat(auth): add Azure AD B2C integration
fix(tenant): resolve data isolation issue
docs(readme): update .NET 10 upgrade instructions
test(api): add integration tests for tenant endpoints
```

## 🔍 Code Review Process

### Reviewer Guidelines

1. **Checklist for Reviewers**
   - [ ] Code builds and runs successfully
   - [ ] All tests pass
   - [ ] Code follows project standards
   - [ ] Architecture principles are maintained
   - [ ] Security implications are considered
   - [ ] Performance impact is assessed
   - [ ] Documentation is adequate

2. **Review Etiquette**
   - Be constructive and respectful
   - Explain the reasoning behind suggestions
   - Focus on the code, not the person
   - Ask questions to understand intent
   - Acknowledge good work and improvements

### Author Guidelines

1. **Before Requesting Review**
   - Self-review your changes
   - Ensure all tests pass
   - Update documentation
   - Consider edge cases and error handling

2. **During Review**
   - Respond to feedback promptly
   - Explain your design decisions
   - Be open to suggestions
   - Update PR based on feedback

## 🏗️ Architecture Guidelines

### Clean Architecture

```
src/
├── SaasBoilerplate.Domain/          # Core business logic
├── SaasBoilerplate.Application/     # Application services
├── SaasBoilerplate.Infrastructure/  # External concerns
└── SaasBoilerplate.WebApi/         # Presentation layer
```

### Multi-Tenancy Patterns

1. **Data Isolation**
   - Use `IMustHaveTenant` interface
   - Implement global query filters
   - Ensure tenant-specific data access

2. **Service Design**
   - Tenant-aware services
   - Proper dependency injection
   - Configuration per tenant

3. **Security**
   - Tenant resolution middleware
   - Authorization checks
   - Data validation

## 🛡️ Security

### Security Guidelines

1. **Never commit secrets** (API keys, connection strings, etc.)
2. **Use environment variables** for configuration
3. **Follow secure coding practices**
4. **Report security vulnerabilities privately**

### Reporting Security Issues

For security vulnerabilities, please:
- Create a private issue or contact maintainers directly
- Do not disclose publicly until patched
- Provide detailed information about the vulnerability

## 📚 Documentation

### Documentation Standards

1. **Code Documentation**
   - XML comments for public APIs
   - Inline comments for complex logic
   - README for each major component

2. **API Documentation**
   - OpenAPI/Swagger specifications
   - Clear endpoint descriptions
   - Example requests and responses

3. **Architecture Documentation**
   - Keep diagrams updated
   - Document design decisions
   - Explain complex patterns

## 🎯 Project Goals and Priorities

### Current Focus Areas

1. **.NET 10 Migration**
   - Upgrade framework dependencies
   - Update to modern patterns
   - Ensure compatibility

2. **Enhanced Multi-Tenancy**
   - Improve data isolation
   - Add tenant management features
   - Performance optimization

3. **Testing and Quality**
   - Increase test coverage
   - Add performance tests
   - Improve CI/CD pipeline

### Contribution Priorities

1. **High Priority**
   - .NET 10 upgrade contributions
   - Security improvements
   - Bug fixes

2. **Medium Priority**
   - New features
   - Documentation improvements
   - Performance optimizations

3. **Low Priority**
   - Code refactoring
   - Tooling improvements
   - Experimental features

## 📞 Getting Help

### Communication Channels

- **GitHub Issues**: For bug reports and feature requests
- **GitHub Discussions**: For general questions and discussions
- **Pull Requests**: For code contributions

### Response Times

- **Bug Reports**: We aim to respond within 3-5 business days
- **Feature Requests**: We'll review and respond within 1 week
- **Pull Requests**: We aim to review within 3-5 business days

## 📄 License

By contributing to this project, you agree that your contributions will be licensed under the same license as the project (MIT License for code, CC BY-SA 4.0 for content).

---

## Quick Start for Contributors

1. Fork the repo and create your branch from `main`
2. If you've added code that should be tested, add tests
3. If you've changed APIs, update the documentation
4. Ensure the test suite passes
5. Make sure your code follows our coding standards
6. Issue that pull request!

Thank you for contributing to the Multi-Tenant SaaS Boilerplate! Your contributions help make this project better for everyone. 🎉
