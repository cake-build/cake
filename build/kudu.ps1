$ProgressPreference='SilentlyContinue'

if (!(Test-Path $env:DEPLOYMENT_TARGET))
{
    EXIT 404
}

if (!(Test-Path $ENV:DEPLOYMENT_SOURCE))
{
    EXIT 404
}

$CAKE_LOG_PATH = "$env:DEPLOYMENT_TARGET\default.htm"
$CAKE_WORK_LOG_PATH = "$env:DEPLOYMENT_TARGET\default.tmp.htm"
$CAKE_BUILDSTATUS_PATH = "$env:DEPLOYMENT_TARGET\buildstatus.svg"
$CAKE_TOOLS_PATH = "$env:DEPLOYMENT_SOURCE\tools"
$CAKE_NUGET_PATH = "$CAKE_TOOLS_PATH\nuget.exe"
$CAKE_EXE_PATH = "$CAKE_TOOLS_PATH\Cake\Cake.exe"
$BUILD_STATUS_IMG = '<img src="buildstatus.svg" />'

if (!(Test-Path $CAKE_TOOLS_PATH))
{
    New-Item $CAKE_TOOLS_PATH
}

# Repord building
'<svg xmlns="http://www.w3.org/2000/svg" width="92" height="20"><linearGradient id="b" x2="0" y2="100%"><stop offset="0" stop-color="#bbb" stop-opacity=".1"/><stop offset="1" stop-opacity=".1"/></linearGradient><mask id="a"><rect width="92" height="20" rx="3" fill="#fff"/></mask><g mask="url(#a)"><path fill="#555" d="M0 0h39v20H0z"/><path fill="#dfb317" d="M39 0h53v20H39z"/><path fill="url(#b)" d="M0 0h92v20H0z"/></g><g fill="#fff" text-anchor="middle" font-family="DejaVu Sans,Verdana,Geneva,sans-serif" font-size="11"><text x="19.5" y="15" fill="#010101" fill-opacity=".3">Kudu</text><text x="19.5" y="14">Kudu</text><text x="64.5" y="15" fill="#010101" fill-opacity=".3">building</text><text x="64.5" y="14">building</text></g></svg>'| Set-Content $CAKE_BUILDSTATUS_PATH -Encoding UTF8

"<html><title>Building $env:SCM_COMMIT_ID</title><body>$BUILD_STATUS_IMG<br><pre>Currently building $env:SCM_COMMIT_ID...</pre></body></html>"|Set-Content $CAKE_LOG_PATH -Encoding UTF8 -PassThru

# Start building
"<html><title>Cake Kudu build log $env:SCM_COMMIT_ID</title><body>$BUILD_STATUS_IMG<br><pre>Cake Kudu build log $env:SCM_COMMIT_ID"|Set-Content $CAKE_WORK_LOG_PATH -Encoding UTF8 -PassThru

Remove-Item "$CAKE_TOOLS_PATH\*" -Recurse -Force -Exclude packages.config,nuget.exe 2>&1|Add-Content $CAKE_WORK_LOG_PATH -Encoding UTF8 -PassThru

if (!(Test-Path $CAKE_NUGET_PATH))
{
    Invoke-WebRequest https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile $CAKE_NUGET_PATH 2>&1|Add-Content $CAKE_WORK_LOG_PATH -Encoding UTF8 -PassThru
}
SET NUGET_EXE=$CAKE_NUGET_PATH

Set-Location $CAKE_TOOLS_PATH
.\nuget.exe install -ExcludeVersion -Verbosity Detailed 2>&1|Add-Content $CAKE_WORK_LOG_PATH -Encoding UTF8 -PassThru
Set-Location $env:DEPLOYMENT_SOURCE

Invoke-Expression "$CAKE_EXE_PATH -version 2>&1|Add-Content $CAKE_WORK_LOG_PATH -Encoding UTF8 -PassThru"

Invoke-Expression "$CAKE_EXE_PATH build.cake -Configuration=debug -Verbosity=Diagnostic -Target=Run-Unit-Tests 2>&1|Add-Content $CAKE_WORK_LOG_PATH -Encoding UTF8 -PassThru"

#Report badge status
if ($LASTEXITCODE -eq 0)
{
    "Build passing!"
    '<svg xmlns="http://www.w3.org/2000/svg" width="92" height="20"><linearGradient id="b" x2="0" y2="100%"><stop offset="0" stop-color="#bbb" stop-opacity=".1"/><stop offset="1" stop-opacity=".1"/></linearGradient><mask id="a"><rect width="92" height="20" rx="3" fill="#fff"/></mask><g mask="url(#a)"><path fill="#555" d="M0 0h39v20H0z"/><path fill="#97CA00" d="M39 0h53v20H39z"/><path fill="url(#b)" d="M0 0h92v20H0z"/></g><g fill="#fff" text-anchor="middle" font-family="DejaVu Sans,Verdana,Geneva,sans-serif" font-size="11"><text x="19.5" y="15" fill="#010101" fill-opacity=".3">Kudu</text><text x="19.5" y="14">Kudu</text><text x="64.5" y="15" fill="#010101" fill-opacity=".3">passing</text><text x="64.5" y="14">passing</text></g></svg>' | Set-Content $CAKE_BUILDSTATUS_PATH -Encoding UTF8
}
else
{
    "Build failed!"
    '<svg xmlns="http://www.w3.org/2000/svg" width="80" height="20"><linearGradient id="b" x2="0" y2="100%"><stop offset="0" stop-color="#bbb" stop-opacity=".1"/><stop offset="1" stop-opacity=".1"/></linearGradient><mask id="a"><rect width="80" height="20" rx="3" fill="#fff"/></mask><g mask="url(#a)"><path fill="#555" d="M0 0h39v20H0z"/><path fill="#e05d44" d="M39 0h41v20H39z"/><path fill="url(#b)" d="M0 0h80v20H0z"/></g><g fill="#fff" text-anchor="middle" font-family="DejaVu Sans,Verdana,Geneva,sans-serif" font-size="11"><text x="19.5" y="15" fill="#010101" fill-opacity=".3">Kudu</text><text x="19.5" y="14">Kudu</text><text x="58.5" y="15" fill="#010101" fill-opacity=".3">failed</text><text x="58.5" y="14">failed</text></g></svg>' | Set-Content $CAKE_BUILDSTATUS_PATH -Encoding UTF8
}

"</pre></body></html>"|Add-Content $CAKE_WORK_LOG_PATH -Encoding UTF8 -PassThru

# Report build log
Remove-Item $CAKE_LOG_PATH
Move-Item -Path $CAKE_WORK_LOG_PATH -Destination $CAKE_LOG_PATH -Force

#Regardless what happens report ok
exit 0