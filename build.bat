@echo off

:Build
cls

echo Restoring NuGet packages for solution...
"tools\nuget\nuget.exe" "restore" "src/Cake.sln"
echo.

if not exist tools\fake\tools\Fake.exe ( 
	echo Installing FAKE...
	"tools\nuget\nuget.exe" "install" "fake" "-OutputDirectory" "tools" "-ExcludeVersion" "-NonInteractive"
	echo.
)

if not exist tools\xunit.runners\tools\xunit.console.exe (
	echo Installing xUnit.net: Runners...
	"tools\nuget\nuget.exe" "install" "xunit.runners" "-OutputDirectory" "tools" "-ExcludeVersion" "-Version" "1.9.2" "-NonInteractive"
	echo.
)

SET TARGET="All"
IF NOT [%1]==[] (set TARGET="%1")
SET BUILDMODE="Release"
IF NOT [%2]==[] (set BUILDMODE="%2")

echo Starting FAKE...
"tools\fake\tools\Fake.exe" "build.fsx" "target=%TARGET%" "buildMode=%BUILDMODE%"

rem Loop the build script.
echo.
set CHOICE=nothing
echo (R)ebuild or (Enter) to exit
set /P CHOICE= 
if /i "%CHOICE%"=="R" goto :Build

:Quit
exit /b %errorlevel%