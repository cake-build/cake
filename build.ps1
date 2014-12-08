Param(
    [string]$Script = "build.cake",
    [string]$Target = "Default",
    [string]$Configuration = "Release",
    [string]$Verbosity = "Verbose"
)

$TOOLS_DIR = Join-Path $PSScriptRoot "tools"
$NUGET_EXE = Join-Path $TOOLS_DIR "nuget.exe"
$CAKE_EXE = Join-Path $TOOLS_DIR "Cake/Cake.exe"

# Make sure NuGet exists where we expect it.
if (!(Test-Path $NUGET_EXE)) {
    Throw "Could not find NuGet.exe"
}

# Restore tools from NuGet.
Start-Process $NUGET_EXE -Wait -NoNewWindow -ArgumentList "install -ExcludeVersion" -WorkingDirectory $TOOLS_DIR | Out-Host

# Make sure that Cake has been installed.
if (!(Test-Path $CAKE_EXE)) {
    Throw "Could not find Cake.exe"
}

# Start Cake
Write-Host
Start-Process $CAKE_EXE -Wait -NoNewWindow -ArgumentList "$Script -target=$Target -configuration=$Configuration -verbosity=$Verbosity" | Out-Host
Write-Host
