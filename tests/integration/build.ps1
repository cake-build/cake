[CmdletBinding()]
Param(
    [string]$Target = "Run-All-Tests",
    [switch]$SkipBuildingCake
)

#####################################################################
# FUNCTIONS
#####################################################################

Function CleanDirectory([string]$Directory) 
{
    if(Test-Path $Directory) {
        Remove-Item -Path $Directory -Recurse -Force
    }
    New-Item -Path $Directory -Type directory | out-null;
}

Function BuildCake([string]$ScriptRoot) 
{
    # Clean the build directory.
    $RootDirectory = (Join-Path $ScriptRoot "../..");
    CleanDirectory((Join-Path $RootDirectory "artifacts"))
    
    # Build Cake.
    try  {
        Push-Location;
        Set-Location $RootDirectory;
        Invoke-Expression "./build.ps1 -Target Copy-Files" | Out-Null;
        if($LASTEXITCODE -ne 0) {
            Throw "An error occured while building Cake.";
        }        
    }
    finally {
        Pop-Location    
    }
}

Function GetCakePath([string]$ScriptRoot)
{
    # Find and return the built directory.
    try {
        Push-Location
        Set-Location (Join-Path $PSScriptRoot "../..");
        Return (Get-ChildItem -Path "./artifacts" -Filter "Cake.exe" -Recurse | Select -First 1).Directory.FullName;
    }
    finally { 
        Pop-Location
    }   
}

#####################################################################
# PREPARATION
#####################################################################

$PSScriptRoot = split-path -parent $MyInvocation.MyCommand.Definition;
$Script = (Join-Path $PSScriptRoot "windows.cake");
$ToolsPath = Join-Path $PSScriptRoot "tools";
$NuGetPath = Join-Path $ToolsPath "nuget.exe";
$NuGetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe";

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

#####################################################################
# BUILD CAKE
#####################################################################

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

# Get the local Cake path. 
$CakePath = Join-Path $ToolsPath "Cake";
$CakeExePath = Join-Path $CakePath "Cake.exe";

# Clean the local Cake path.
if(!$SkipBuildingCake.IsPresent) {
  CleanDirectory($CakePath);
  Copy-Item "$BuiltCakePath/*.*" $CakePath -Recurse
}

# Ensure that Cake can be found where we expect it to.
if(!(Test-Path $CakeExePath)) {
  Throw "Could not locate Cake at $CakeExePath.";
}

#####################################################################
# SETUP ENVIRONMENT
#####################################################################

$Env:MyEnvironmentVariable = "Hello World";

#####################################################################
# RUN TESTS
#####################################################################

# Run tests using new Cake.
&$CakeExePath "--version"
Write-Output "Running integration tests...";
&$CakeExePath "$Script" "--target=$Target" "--verbosity=quiet" "--platform=windows" "--customarg=hello"
Write-Output "";
