@echo off

:Build
cls

if not exist tools\Cake\Cake.exe ( 
	echo Installing Cake...
	tools\nuget\nuget.exe install Cake -OutputDirectory tools -ExcludeVersion -NonInteractive -NoCache
	echo.
)

if not exist tools\xunit.runners\tools\xunit.console.exe (
	echo Installing xUnit.net: Runners...
	tools\nuget\nuget.exe install xunit.runners -OutputDirectory tools -ExcludeVersion -Version 1.9.2 -NonInteractive
	echo.
)

SET TARGET="All"
IF NOT [%1]==[] (set TARGET="%1")
SET BUILDMODE="Release"
IF NOT [%2]==[] (set BUILDMODE="%2")
SET LOCALBUILD="false"
IF NOT [%3]==[] (set LOCALBUILD="true")

echo Starting Cake...
tools\Cake\Cake.exe build.cake -target=%TARGET% -configuration=%BUILDMODE% -verbosity=diagnostic -local=%LOCALBUILD%

:Quit
exit /b %errorlevel%