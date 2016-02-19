$TOOLS_DIR = Join-Path $PSScriptRoot "tools"
$NUGET_EXE = Join-Path $TOOLS_DIR "nuget.exe"

# Try download NuGet.exe if do not exist.
if (!(Test-Path $NUGET_EXE)) {
    Invoke-WebRequest -Uri https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile $NUGET_EXE
}

# Make sure NuGet exists where we expect it.
if (!(Test-Path $NUGET_EXE)) {
    Throw "Could not find NuGet.exe"
}

$CakePath = Join-Path $PSScriptRoot "Tools/Cake/";
$CAKE_EXE = Join-Path $CakePath "bin/Cake.exe";
if(!(Test-Path $CakePath))
{
  Write-Output "Building Cake from source..."
  Push-Location
  Set-Location (Join-Path $PSScriptRoot "../");
  $BuildCommand = "./build.ps1 -Target Copy-Files -Output $CakePath";
  Invoke-Expression $BuildCommand | Out-Null;
  Pop-Location
}

# Run all tests.
Write-Host ""
$Files =ls *Tests.cake;
ForEach($File in $Files)
{
  $Script = $File.Name

  Write-Host "Running " -f "white" -nonewline
  Write-Host $Script -f "yellow" -nonewline
  Write-Host "..." -f "white" -nonewline

  Invoke-Expression "$CAKE_EXE `"$Script`" -verbosity=`"q`" -target=`"$Target`"" -OutVariable Output -ErrorVariable Errorz *> $null
  $ExitCode = $LASTEXITCODE;

  # Was there any errors?
  if($ExitCode -eq 0)
  {
      Write-Host(" [Pass] ") -foreground "green"
  }
  else
  {
      $Output = "";
      $Errorz | ForEach-Object {
        $Temp =  $_.ToString().Trim() ;
        if(!([string]::IsNullOrWhiteSpace($Temp))) {
          $Output = $Output + "`r`n" + $_.ToString()
        }
      };

      $Output = $Output.ToString().Substring($Output.IndexOf(':') + 1).Trim();
      $Parts = $Output.Split("£".ToCharArray(), [System.StringSplitOptions]::RemoveEmptyEntries);

      Write-Host(" [Fail] ") -foreground "red"
      Write-Host("`r`nFailing Test:") -foreground "white"
      Write-Host $Parts[0] -foreground "yellow"
      Write-Host("`r`nError:") -foreground "white"
      Write-Host $Parts[1] -foreground "yellow"
      Write-Host ""

      Return;
  }
}

Write-Host ""
exit $ExitCode