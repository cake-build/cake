:Build
call build.cmd

echo.
set CHOICE=nothing
echo (R)ebuild or (Enter) to exit
set /P CHOICE= 
if /i "%CHOICE%"=="R" goto :Build

:Quit
exit /b %errorlevel%