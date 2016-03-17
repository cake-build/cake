[CmdletBinding()]
Param(
    [string]$Target = "Run-All-Tests",
    [switch]$SkipBuildingCake
)

$PSScriptRoot = split-path -parent $MyInvocation.MyCommand.Definition;
$ToolsPath = Join-Path $PSScriptRoot "tools";
$NuGetPath = Join-Path $ToolsPath "nuget.exe";
$NuGetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe";

# Load utilities.
. (Join-Path $PSScriptRoot "utilities.ps1");

# Make sure the tools directory exist.
if(!(Test-Path $ToolsPath)) {
    Write-Verbose "Creating tools directory...";
    New-Item -Path $ToolsPath -Type directory | out-null;
}

# Make sure NuGet exist.
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

# Get the path to the local Cake.
$CakePath = (Join-Path $ToolsPath "Cake");

# Clean the local Cake path.
if(!$SkipBuildingCake.IsPresent) {
  CleanDirectory($CakePath);
  Copy-Item "$BuiltCakePath/*.*" $CakePath -Recurse
}

# Run tests using new Cake.
Write-Output "Running integration tests using new Cake...";
$CakeExePath = Join-Path $CakePath "Cake.exe";
&$CakeExePath "--version"
&$CakeExePath "build.cake" "--target=$Target" "--verbosity=quiet" "--customarg=hello"
Write-Output "";
