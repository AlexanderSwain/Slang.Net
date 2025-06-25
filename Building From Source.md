# Building From Source

**For Debugging**
```powershell
& dotnet pack "src\Slang.Net\Slang.Net.csproj" --configuration Debug /p:Platform=All 
```

**For Release**
```powershell
& dotnet pack "src\Slang.Net\Slang.Net.csproj" --configuration Release /p:Platform=All 
```

# Testing changes to Slang.Net with Sample Projects

- Make changes, Build, and Pack (using instructions above)
- Install Package (Be sure to replace 0.0.4 with correct version number)
    - **Option 1: Using dotnet add package (Recommended)**
      ```powershell
      cd "Samples\Slang.Net.Samples.SimpleCompileTest"
      dotnet add package Slang.Net --version 0.0.4 --source "..\..\src\Slang.Net\bin\Debug\net9.0"
      ```
    - **Option 2: Using local package source**
      ```powershell
      # Add local source
      dotnet nuget add source "src\Slang.Net\bin\Debug\net9.0" --name "LocalSlangNet"
      
      # Install package
      cd "Samples\Slang.Net.Samples.SimpleCompileTest"
      dotnet add package Slang.Net --version 0.0.4 --source "LocalSlangNet"
      ```
    - **Option 3: Manual PackageReference (if above options fail)**
      ```xml
      <PackageReference Include="Slang.Net" Version="0.0.4">
          <HintPath>..\..\src\Slang.Net\bin\Debug\net9.0\Slang.Net.0.0.4.nupkg</HintPath>
      </PackageReference>
      ```
- Build and run the Sample project to test changes