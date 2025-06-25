# Building From Source

## Prerequisites
- Visual Studio 2022 (Preview) with C++/CLI support
- .NET 9.0 SDK
- PowerShell

## Build Process
1. Clone the repository
2. Run the build script from the root directory:
   ```powershell
   .\build.ps1
   ```

This will automatically:
- Download the Slang SDK if needed
- Build native dependencies for all platforms (x64/ARM64)
- Build the C++/CLI wrapper for all platforms
- Build the C# wrapper library
- Create the NuGet package

## Building Individual Components

### Native Dependencies
```powershell
cd src\Native
.\build.ps1
```

### C++/CLI Wrapper
```powershell
cd src\Slang.Net.CPP
.\build.ps1
```

### C# Wrapper
```powershell
cd src\Slang.Net
.\build.ps1
```

## Creating NuGet Package

To create the NuGet package manually:
```powershell
cd src\Slang.Net
& "${env:ProgramFiles}\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\MSBuild.exe" /t:Pack /p:Configuration=Debug /p:Platform=x64 /p:IsPacking=true
```

**Important**: Use MSBuild (not `dotnet pack`) to avoid C++/CLI evaluation issues.

## Using the Local Package

After building, you can install the local NuGet package in your projects:

1. **Option 1: Using PackageReference with RestoreSources (Recommended)**
   ```xml
   <ItemGroup>
     <PackageReference Include="Slang.Net" Version="0.0.4" />
   </ItemGroup>

   <PropertyGroup>
     <RestoreSources>$(RestoreSources);path\to\Slang.Net\src\Slang.Net\bin\Debug\net9.0;https://api.nuget.org/v3/index.json</RestoreSources>
   </PropertyGroup>
   ```

2. **Option 2: Using dotnet add package with source**
   ```powershell
   dotnet add package Slang.Net --version 0.0.4 --source "path\to\Slang.Net\src\Slang.Net\bin\Debug\net9.0"
   ```

3. **Option 3: Add local package source**
   ```powershell
   dotnet nuget add source "path\to\Slang.Net\src\Slang.Net\bin\Debug\net9.0" --name "LocalSlangNet"
   dotnet add package Slang.Net --version 0.0.4 --source "LocalSlangNet"
   ```

**Note**: After installing a new version of the local package, you may need to clear the NuGet cache:
```powershell
dotnet nuget locals all --clear
```

## Testing the Package

To test the package with the included sample:
```powershell
cd Samples\Slang.Net.Samples.SimpleCompileTest
dotnet nuget locals all --clear  # Clear cache if needed
dotnet restore --force --no-cache
dotnet build
dotnet run
```

The sample project is already configured with the local package source and will automatically use the locally built package.