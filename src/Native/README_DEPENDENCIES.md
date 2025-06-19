# Slang Native Dependencies

This directory is used to download and store the Slang SDK dependencies required by the Slang.Net project.

## How it works

1. The `download-slang-sdk.ps1` script automatically downloads and extracts the Slang SDK during the build process.
2. The script is called as a pre-build event in the SlangNative.vcxproj file.
3. Downloaded files are stored in the `EmbeddedLLVM` directory, but they are not committed to the repository.

## Manual download

If you need to manually download the Slang SDK, you can run the script directly:

```powershell
.\download-slang-sdk.ps1 -Platform x64
```

Available parameters:
- `-SlangVersion`: The version of Slang to download (default: "2025.10.3")
- `-Platform`: The target platform (x64, x86, ARM64)
- `-Force`: Forces re-download even if files already exist

## Troubleshooting

If you encounter build issues related to missing Slang dependencies:

1. Delete the `EmbeddedLLVM` directory
2. Run the download script manually with the `-Force` parameter
3. Rebuild the project

## Structure

After downloading, the directory structure will look like:

```
EmbeddedLLVM/
  ├─ downloads/
  │   └─ slang-2025.10.3-windows.zip
  └─ slang-2025.10.3-windows/
      ├─ x64/
      │   ├─ bin/
      │   ├─ lib/
      │   └─ include/
      └─ ARM64/
          ├─ bin/
          ├─ lib/
          └─ include/
```
