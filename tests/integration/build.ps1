[CmdletBinding()]
Param(
    [string]$Target = "Run-All-Tests",
    [switch]$SkipBuildingCake
)

$Exclusive = ''
if ($SkipBuildingCake.IsPresent)
{
    $Exclusive = '--exclusive'
}

$PSScriptRoot = split-path -parent $MyInvocation.MyCommand.Definition
$RootDirectory = (Join-Path $PSScriptRoot "../..")


# Build Cake.
try  {
    Push-Location
    Set-Location $RootDirectory
    & "$RootDirectory\build.ps1" --target="Run-Integration-Tests" --integration-tests-target="$Target" $Exclusive
    if($LASTEXITCODE -ne 0) {
        Throw "An error occurred while building Cake."
    }
}
finally {
    Pop-Location
}