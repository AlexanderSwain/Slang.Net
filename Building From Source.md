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
    - ```powershell
        Install-Package src\Slang.Net\bin\Debug\net9.0\Slang.Net.0.0.4.nupkg
      ```
- Build and run the Sample project to test changes