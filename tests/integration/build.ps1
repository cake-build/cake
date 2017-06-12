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
    New-Item -Path $Directory -Type Directory | Out-Null
}

Function BuildCake([string]$ScriptRoot)
{
    # Clean the build directory.
    $RootDirectory = (Join-Path $ScriptRoot "../..")
    CleanDirectory((Join-Path $RootDirectory "artifacts"))

    # Build Cake.
    try  {
        Push-Location
        Set-Location $RootDirectory
        Invoke-Expression "./build.ps1 -Target Copy-Files" | Out-Null
        if($LASTEXITCODE -ne 0) {
            Throw "An error occured while building Cake."
        }
    }
    finally {
        Pop-Location
    }
}

Function GetCakePath([string]$ScriptRoot)
{
    Return (Get-ChildItem -Path (Join-Path $ScriptRoot "../../artifacts" -Resolve) -Filter "Cake.exe" -Recurse | Sort-Object -Property LastWriteTime -Descending | Select -First 1).Directory.FullName
}

Function GetCakeCoreCLRPath([string]$ScriptRoot)
{
    Return (Get-ChildItem -Path (Join-Path $ScriptRoot "../../artifacts" -Resolve) -Filter "Cake.dll" -Recurse | Sort-Object -Property LastWriteTime -Descending | Select -First 1).Directory.FullName
}

#####################################################################
# PREPARATION
#####################################################################

$PSScriptRoot = split-path -parent $MyInvocation.MyCommand.Definition
$Script = (Join-Path $PSScriptRoot "windows.cake")
$MonoScript = (Join-Path $PSScriptRoot "build.cake")
$ToolsPath = Join-Path $PSScriptRoot "tools"
$NuGetPath = Join-Path $ToolsPath "nuget.exe"
$DotNetChannel = "preview"
$DotNetVersion = "1.0.4";
$DotNetInstallerUri = "https://dot.net/v1/dotnet-install.ps1";
$NuGetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"

# Make sure the tools directory exist.
if(!(Test-Path $ToolsPath)) {
    Write-Verbose "Creating tools directory..."
    New-Item -Path $ToolsPath -Type Directory | Out-Null
}

###########################################################################
# INSTALL .NET CORE CLI
###########################################################################

Function Remove-PathVariable([string]$VariableToRemove)
{
    $path = [Environment]::GetEnvironmentVariable("PATH", "User")
    if ($path -ne $null)
    {
        $newItems = $path.Split(';', [StringSplitOptions]::RemoveEmptyEntries) | Where-Object { "$($_)" -inotlike $VariableToRemove }
        [Environment]::SetEnvironmentVariable("PATH", [System.String]::Join(';', $newItems), "User")
    }

    $path = [Environment]::GetEnvironmentVariable("PATH", "Process")
    if ($path -ne $null)
    {
        $newItems = $path.Split(';', [StringSplitOptions]::RemoveEmptyEntries) | Where-Object { "$($_)" -inotlike $VariableToRemove }
        [Environment]::SetEnvironmentVariable("PATH", [System.String]::Join(';', $newItems), "Process")
    }
}

# Get .NET Core CLI path if installed.
$FoundDotNetCliVersion = $null
if (Get-Command dotnet -ErrorAction SilentlyContinue) {
    $FoundDotNetCliVersion = dotnet --version
}

if($FoundDotNetCliVersion -ne $DotNetVersion) {
    $InstallPath = Join-Path $PSScriptRoot ".dotnet"
    if (!(Test-Path $InstallPath)) {
        mkdir -Force $InstallPath | Out-Null
    }
    (New-Object System.Net.WebClient).DownloadFile($DotNetInstallerUri, "$InstallPath\dotnet-install.ps1")
    & $InstallPath\dotnet-install.ps1 -Channel $DotNetChannel -Version $DotNetVersion -InstallDir $InstallPath

    Remove-PathVariable "$InstallPath"
    $env:PATH = "$InstallPath;$env:PATH"
    $env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
    $env:DOTNET_CLI_TELEMETRY_OPTOUT=1
}

dotnet --info

#####################################################################
# BUILD CAKE
#####################################################################

# Build Cake.
if(!$SkipBuildingCake.IsPresent) {
    Write-Host "Building Cake..."
    BuildCake($PSScriptRoot)
}

# Get the built Cake path.
$BuiltCakePath = GetCakePath($PSScriptRoot)
if([string]::IsNullOrWhiteSpace($BuiltCakePath)) {
    Throw "Could not resolve built Cake path."
}

$BuiltCakeCoreCLRPath = GetCakeCoreCLRPath($PSScriptRoot)
if([string]::IsNullOrWhiteSpace($BuiltCakeCoreCLRPath)) {
    Throw "Could not resolve built Cake CoreCLR path."
}

# Get the local Cake path.
$CakePath = Join-Path $ToolsPath "Cake"
$CakeCoreCLRPath = Join-Path $ToolsPath "Cake.CoreCLR"
$CakeExePath = Join-Path $CakePath "Cake.exe"
$CakeDllPath = Join-Path $CakeCoreCLRPath "Cake.dll"

# Clean the local Cake path.
if(!$SkipBuildingCake.IsPresent) {
  CleanDirectory($ToolsPath)
  New-Item -Path $CakePath -Type Directory | Out-Null
  New-Item -Path $CakeCoreCLRPath -Type Directory | Out-Null

  Get-ChildItem -Path $BuiltCakePath | Copy-Item -Destination $CakePath -Recurse -Container
  Get-ChildItem -Path $BuiltCakeCoreCLRPath | Copy-Item -Destination $CakeCoreCLRPath -Recurse -Container
}

# Ensure that Cake can be found where we expect it to.
if(!(Test-Path $CakeExePath)) {
  Throw "Could not locate Cake at $CakeExePath."
}

# Ensure that Cake CoreCLR can be found where we expect it to.
if(!(Test-Path $CakeDllPath)) {
  Throw "Could not locate Cake CoreCLR at $CakeDllPath."
}

# Make sure NuGet exist.
if (!(Test-Path $NuGetPath)) {
    Write-Verbose "Downloading NuGet..."
    (New-Object System.Net.WebClient).DownloadFile($NuGetUrl, $NuGetPath)
    if (!(Test-Path $NuGetPath)) {
        Throw "Could not find NuGet.exe"
    }
}


#####################################################################
# SETUP ENVIRONMENT
#####################################################################

$Env:MyEnvironmentVariable = "Hello World"

#####################################################################
# RUN TESTS
#####################################################################

[int] $TestResult = 0

# Run tests using new Cake.
&$CakeExePath "--version"
Write-Output "Running integration tests..."
&$CakeExePath "$Script" "--target=$Target" "--verbosity=quiet" "--platform=windows" "--customarg=hello"
$TestResult+=$LASTEXITCODE
Write-Output ""

# Run tests using new Cake.
&$CakeExePath "--version"
Write-Output "Running integration tests mono scripting..."
&$CakeExePath "$MonoScript" --mono "--target=$Target" "--verbosity=quiet" "--platform=windows" "--customarg=hello"
$TestResult+=$LASTEXITCODE
Write-Output ""

# Run tests using new Cake.
&dotnet $CakeDllPath "--version"
Write-Output "Running CoreCLR integration tests..."
&dotnet $CakeDllPath "$Script" "--target=$Target" "--verbosity=quiet" "--platform=windows" "--customarg=hello"
$TestResult+=$LASTEXITCODE
Write-Output ""

Exit $TestResult