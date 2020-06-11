Task("Cake.Core.Diagnostics.LogEscapedBraces")
    .Does(() => {
    Context.Log.Debug(
        Verbosity.Normal,
        string.Format("Executing: {0}", "if ($LASTEXITCODE -gt 0) {{ throw \"script failed with exit code $LASTEXITCODE\" }}")
        );
});


Task("Cake.Core.Diagnostics")
    .IsDependentOn("Cake.Core.Diagnostics.LogEscapedBraces");