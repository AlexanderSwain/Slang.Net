# Building From Source

**For Debugging**
```powershell
& dotnet pack "src\Slang.Net\Slang.Net.csproj" --configuration Debug /p:Platform=All 
```

**For Release**
```powershell
& dotnet pack "src\Slang.Net\Slang.Net.csproj" --configuration Release /p:Platform=All 
```