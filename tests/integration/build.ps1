[CmdletBinding()]
Param(
    [switch]$SkipBuildingCake
)

$PSScriptRoot = split-path -parent $MyInvocation.MyCommand.Definition;

# Load utilities.
. (Join-Path $PSScriptRoot "utilities.ps1");

# Make sure the tools directory exist.
$ToolsPath = Join-Path $PSScriptRoot "tools";
if(!(Test-Path $ToolsPath)) {
    Write-Verbose "Creating tools directory...";
    New-Item -Path $ToolsPath -Type directory | out-null;
}

# Make sure NuGet exist.
$NuGetPath = Join-Path $ToolsPath "nuget.exe";
$NuGetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe";
if (!(Test-Path $NuGetPath)) {
    Write-Verbose "Downloading NuGet..."
    (New-Object System.Net.WebClient).DownloadFile($NuGetUrl, $NuGetPath);
    if (!(Test-Path $NuGetPath)) {
        Throw "Could not find NuGet.exe";
    }
}

# Build Cake.
if(!$SkipBuildingCake.IsPresent) {
    Write-Host "Building Cake..."
    BuildCake($PSScriptRoot);
}

# Get the built Cake path.
$BuiltCakePath = GetCakePath($PSScriptRoot);
if([string]::IsNullOrWhiteSpace($BuiltCakePath)) {
    Throw "Could not resolve built Cake path."
}

# Clean the local Cake path.
$CakePath = (Join-Path $ToolsPath "Cake");
CleanDirectory($CakePath);
Copy-Item "$BuiltCakePath/*.*" $CakePath -Recurse

# Run tests using new Cake.
Write-Output "Running integration tests using new Cake...";
$CakeExePath = Join-Path $CakePath "Cake.exe";
&$CakeExePath "--version"
