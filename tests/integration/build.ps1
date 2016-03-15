$PSScriptRoot = split-path -parent $MyInvocation.MyCommand.Definition;

$TOOLS_DIR = Join-Path $PSScriptRoot "tools";
$NUGET_EXE = Join-Path $TOOLS_DIR "nuget.exe";
$NUGET_URL = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe";

#################################################################################

Function CleanDirectory([string]$Directory) {
    if(Test-Path $Directory) {
        Remove-Item -Path $Directory -Recurse -Force
    }
    New-Item -Path $Directory -Type directory | out-null;
}

Function GetCakeDirectory() {
    Push-Location
    Set-Location (Join-Path $PSScriptRoot "../..");
    $OriginalCakePath = (Get-ChildItem -Path "./build" -Filter "Cake.exe" -Recurse | Select -First 1).Directory.FullName;
    Pop-Location
    Return $OriginalCakePath;
}

#################################################################################

# Tools directory
if(!(Test-Path $TOOLS_DIR)) {
    Write-Verbose -Message "Creating tools directory...";
    New-Item -Path $TOOLS_DIR -Type directory | out-null;
}

# NuGet
if (!(Test-Path $NUGET_EXE)) {
    (New-Object System.Net.WebClient).DownloadFile($NUGET_URL, $NUGET_EXE);
    if (!(Test-Path $NUGET_EXE)) {
        Throw "Could not find NuGet.exe";
    }
}

# Build Cake.
$RootDirectory = (Join-Path $PSScriptRoot "../..");
CleanDirectory((Join-Path $RootDirectory "build"))
Push-Location;
Set-Location $RootDirectory;
Write-Output "Building Cake...";
Invoke-Expression "./build.ps1 -Target Copy-Files" | Out-Null;
if($LASTEXITCODE -ne 0) {
    Throw "An error occured while building Cake.";
}
Pop-Location

# Clean directory
CleanDirectory((Join-Path $TOOLS_DIR "Cake"));
$CakePath = GetCakeDirectory; 
Write-Host "Cake Directory: $CakePath";

# Copy files

# Run tests using new Cake.
Write-Output "Running integration tests using new Cake.";

