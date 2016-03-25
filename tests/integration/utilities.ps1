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
    CleanDirectory((Join-Path $RootDirectory "build"))
    
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
       
    Return GetCakePath($ScriptRoot);
}

Function GetCakePath([string]$ScriptRoot)
{
    # Find and return the built directory.
    try {
        Push-Location
        Set-Location (Join-Path $PSScriptRoot "../..");
        Return (Get-ChildItem -Path "./build" -Filter "Cake.exe" -Recurse | Select -First 1).Directory.FullName;
    }
    finally { 
        Pop-Location
    }   
}
