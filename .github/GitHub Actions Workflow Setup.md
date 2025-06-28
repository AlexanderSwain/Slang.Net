# GitHub Actions Workflow Setup

This repository includes a GitHub Actions workflow file (`nuget-publish.yml`) that automatically builds and publishes the Slang.Net NuGet package when code is pushed to the `production` branch.

## Setup Instructions

### 1. NuGet API Key

To publish packages to NuGet.org, you need to set up a secret in your GitHub repository:

1. Go to [NuGet.org](https://www.nuget.org) and sign in
2. Go to your account settings and create a new API key
3. In your GitHub repository, go to **Settings** → **Secrets and variables** → **Actions**
4. Click **New repository secret**
5. Name: `NUGET_API_KEY`
6. Value: Your NuGet API key
7. Click **Add secret**

### 2. Branch Protection (Optional)

Consider setting up branch protection rules for your `production` branch:

1. Go to **Settings** → **Branches**
2. Click **Add rule**
3. Branch name pattern: `production`
4. Enable desired protection rules (require PR reviews, status checks, etc.)

### 3. Workflow Triggers

The workflow runs on:
- **Push to production branch**: Automatically triggers when code is pushed to `production`
- **Manual trigger**: Can be manually triggered from the Actions tab using `workflow_dispatch`

### 4. Build Process

The workflow follows these steps:
1. **Checkout code** with submodules
2. **Setup .NET 9.0** (preview)
3. **Setup MSBuild** (latest Visual Studio)
4. **Build Native Dependencies** using `all-platforms.ps1`
5. **Build C++/CLI Wrapper** using `all-platforms.ps1`
6. **Build C# Wrapper** using `all-platforms.ps1`
7. **Create NuGet Package** (Release configuration)
8. **Publish to NuGet.org** using the API key
9. **Upload artifacts** for later download if needed

### 5. Monitoring

- Check the **Actions** tab in your GitHub repository to monitor workflow runs
- Failed builds will send notifications (if configured)
- Artifacts are retained for 30 days

## Troubleshooting

### Common Issues

1. **Build Failures**: Check the build logs in the Actions tab
2. **NuGet Publish Failures**: Verify your API key is correct and has the right permissions
3. **Missing Dependencies**: Ensure all required tools are available in the Windows runner

### Manual Workflow Trigger

You can manually trigger the workflow:
1. Go to the **Actions** tab
2. Select **Build and Publish NuGet Package**
3. Click **Run workflow**
4. Select the branch and click **Run workflow**
