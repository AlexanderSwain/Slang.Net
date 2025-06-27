# Building From Source

## Generating the Nuget package

**Building Slang Native**

- Step 1: Make sure you're in the source directory
```powershell
cd src\
```

- Step 2: Run the build script for Slang Native
```powershell
.\all-platforms.ps1 -Script Native\build.ps1
```

**Building Slang.Net.CPP**

- Step 1: Make sure you're in the source directory
```powershell
cd src\
```

- Step 2: Run the build script for Slang.Net.CPP
```powershell
.\all-platforms.ps1 -Script Slang.Net.CPP\build.ps1
```

**Building Slang.Net**

- Step 1: Make sure you're in the source directory
```powershell
cd src\
```

- Step 2: Run the build script for Slang.Net.CPP
```powershell
.\all-platforms.ps1 -Script Slang.Net\build.ps1
```

**Building the Nuget package**

- Step 1: Make sure you're in the source directory
```powershell
cd src\
```

- Step 2: Run the build script for Slang.Net.CPP
**For Debugging**
```powershell
dotnet pack .\Slang.Net --configuration Debug --verbosity normal --no-build
```

**For Release**
```powershell
dotnet pack .\Slang.Net --configuration Release --verbosity normal --no-build
```

- The nuget package will be generated in the Slang.Net\Builds sub-folders