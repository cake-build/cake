#!/usr/bin/env pwsh
$DotNetInstallerUri = 'https://dot.net/v1/dotnet-install.ps1';
$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent

# Make sure tools folder exists
$ToolPath = Join-Path $PSScriptRoot "tools"
if (!(Test-Path $ToolPath)) {
    Write-Verbose "Creating tools directory..."
    New-Item -Path $ToolPath -Type directory | out-null
}

###########################################################################
# INSTALL .NET CORE CLI
###########################################################################

Function Remove-PathVariable([string]$VariableToRemove)
{
  $path = [Environment]::GetEnvironmentVariable("PATH", "User")
  $newItems = $path.Split(';') | Where-Object { $_.ToString() -inotlike $VariableToRemove }
  [Environment]::SetEnvironmentVariable("PATH", [System.String]::Join(';', $newItems), "User")
  $path = [Environment]::GetEnvironmentVariable("PATH", "Process")
  $newItems = $path.Split(';') | Where-Object { $_.ToString() -inotlike $VariableToRemove }
  [Environment]::SetEnvironmentVariable("PATH", [System.String]::Join(';', $newItems), "Process")
}

$InstallPath = Join-Path $PSScriptRoot ".dotnet"
if (!(Test-Path $InstallPath)) {
    mkdir -Force $InstallPath | Out-Null;
}
(New-Object System.Net.WebClient).DownloadFile($DotNetInstallerUri, "$InstallPath\dotnet-install.ps1");
& $InstallPath\dotnet-install.ps1 -JSonFile global.json -InstallDir $InstallPath;

Remove-PathVariable "$InstallPath"
$env:PATH = "$InstallPath;$env:PATH"
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
$env:DOTNET_CLI_TELEMETRY_OPTOUT=1

###########################################################################
# RUN BUILD SCRIPT
###########################################################################

dotnet run --project build/Build.csproj -- $args
exit $LASTEXITCODE;