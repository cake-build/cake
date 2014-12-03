@echo off

:Build
cls

if not exist tools\Cake\Cake.exe ( 
	echo Installing Cake...
	tools\nuget\nuget.exe install Cake -OutputDirectory tools -ExcludeVersion -NonInteractive -NoCache -Source https://www.myget.org/F/cake/ -Prerelease
	echo.
)

if not exist tools\xunit.runners\tools\xunit.console.exe (
	echo Installing xUnit.net: Runners...
	tools\nuget\nuget.exe install xunit.runners -OutputDirectory tools -Prerelease -ExcludeVersion -NonInteractive -Version 2.0.0-beta4-build2738
	echo.
)

SET TARGET="Default"
IF NOT [%1]==[] (set TARGET="%1")
SET BUILDMODE="Release"
IF NOT [%2]==[] (set BUILDMODE="%2")

echo Starting Cake...
tools\Cake\Cake.exe build.cake -target=%TARGET% -configuration=%BUILDMODE% -verbosity=diagnostic

:Quit
exit /b %errorlevel%