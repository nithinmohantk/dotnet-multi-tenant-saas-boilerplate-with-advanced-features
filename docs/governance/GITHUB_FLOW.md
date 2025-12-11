# GitHub Flow Branch Protection Configuration

Complete configuration bundle for implementing GitHub Flow with enterprise-grade security and quality controls.

## Repository Structure

```
.github/
├── rulesets/
│   ├── main-branch-protection.json
│   ├── develop-branch-protection.json
│   ├── release-branch-protection.json
│   └── feature-branch-protection.json
├── workflows/
│   ├── ci.yml
│   ├── cd-dev.yml
│   ├── cd-staging.yml
│   ├── cd-prod.yml
│   └── required-checks.yml
├── CODEOWNERS
├── PULL_REQUEST_TEMPLATE.md
├── ISSUE_TEMPLATE/
└── dependabot.yml
```

## GitHub Flow Workflow Guide

### Branch Naming Convention
```
feature/JIRA-123-short-description
bugfix/JIRA-456-issue-description
hotfix/critical-security-patch
release/v1.2.3
docs/update-readme
refactor/improve-performance
test/add-integration-tests
chore/update-dependencies
```

### Commit Message Convention (Conventional Commits)
```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]

Types:
- feat: A new feature
- fix: A bug fix
- docs: Documentation only changes
- style: Code style changes (formatting, etc)
- refactor: Code refactoring
- perf: Performance improvements
- test: Adding or updating tests
- build: Build system changes
- ci: CI/CD changes
- chore: Other changes that don't modify src or test files
- revert: Reverts a previous commit

Examples:
feat(api): add user authentication endpoint
fix(database): resolve connection timeout issue
docs: update API documentation for v2
perf(query): optimize database query for user search
```

### GitHub Flow Process

1. **Create a branch from main**
   ```bash
   git checkout main
   git pull origin main
   git checkout -b feature/JIRA-123-new-feature
   ```

2. **Make changes and commit**
   ```bash
   git add .
   git commit -s -m "feat(api): add new endpoint for user management"
   ```

3. **Push to GitHub**
   ```bash
   git push origin feature/JIRA-123-new-feature
   ```

4. **Create Pull Request**
   - Fill out PR template completely
   - Link related issues
   - Request reviews from CODEOWNERS
   - Add appropriate labels

5. **Code Review Process**
   - Address review comments
   - Push additional commits
   - Ensure all checks pass
   - Get required approvals (2 for main branch)

6. **Merge to main**
   - Squash and merge (preferred for clean history)
   - Delete feature branch after merge
   - Automated deployment to dev environment

7. **Deploy to production**
   - Create release from main
   - Automated deployment via CD pipeline
   - Monitor deployment

## Applying Rulesets via GitHub CLI

```bash
# Install GitHub CLI if not already installed
# https://cli.github.com/

# Authenticate
gh auth login

# Create rulesets from JSON files
gh api \
  --method POST \
  -H "Accept: application/vnd.github+json" \
  /repos/OWNER/REPO/rulesets \
  --input .github/rulesets/main-branch-protection.json

gh api \
  --method POST \
  -H "Accept: application/vnd.github+json" \
  /repos/OWNER/REPO/rulesets \
  --input .github/rulesets/develop-branch-protection.json

gh api \
  --method POST \
  -H "Accept: application/vnd.github+json" \
  /repos/OWNER/REPO/rulesets \
  --input .github/rulesets/release-branch-protection.json

gh api \
  --method POST \
  -H "Accept: application/vnd.github+json" \
  /repos/OWNER/REPO/rulesets \
  --input .github/rulesets/tag-protection.json

# List all rulesets
gh api \
  -H "Accept: application/vnd.github+json" \
  /repos/OWNER/REPO/rulesets
```

## Status Check Configuration

Required status checks should match your CI/CD pipeline jobs. Update the `required_status_checks` array based on your actual workflow job names.

**Example workflow job names:**
- `build` 
- `unit-tests` 
- `integration-tests` 
- `e2e-tests` 
- `code-quality / sonarcloud` 
- `security / sast` 
- `security / dependency-check` 
- `security / container-scan` 
- `security / secrets-scan` 
- `security / codeql` 
- `architecture-tests` 
- `performance-tests` 
- `terraform / validate` 
- `terraform / security` 
- `license-check` 

## Best Practices Summary

1. **Always work in feature branches** - Never commit directly to main
2. **Keep branches short-lived** - Merge within 2-3 days
3. **Write meaningful commit messages** - Follow conventional commits
4. **Request reviews early** - Don't wait until everything is perfect
5. **Respond to reviews promptly** - Keep the conversation flowing
6. **Keep PRs focused** - One feature/fix per PR
7. **Delete merged branches** - Keep repository clean
8. **Sign your commits** - Enable commit signing
9. **Run tests locally** - Before pushing
10. **Update documentation** - With code changes

## Monitoring and Compliance

- Review branch protection effectiveness monthly
- Audit bypass actors quarterly
- Review CODEOWNERS annually
- Update required checks as pipeline evolves
- Monitor merge times and bottlenecks
- Track security scan findings
- Review and update rulesets with team feedback
