Task("Cake.Core.Scripting.AddinDirective.LoadNetStandardAddin")
    .Does(() =>
{
    DotNetCorePack($"{Paths.Resources}/Cake.Core/Scripting/netstandard2.addin/netstandard2.addin.csproj",
        new DotNetCorePackSettings {
            Configuration = "Release"
        });

    var script = $@"#addin nuget:{Paths.Resources}/Cake.Core/Scripting/netstandard2.addin/bin/Release?package=netstandard2.addin&version=1.0.0
        Information(""Magic number: {0}"", GetMagicNumber(false));
        Information(""The answer to life: {0}"", TheAnswerToLife);
    ";

    CakeExecuteExpression(script,
    	new CakeSettings {
    		EnvironmentVariables = new Dictionary<string, string>{
    			{"CAKE_PATHS_ADDINS", $"{Paths.Temp}/tools/Addins"},
    			{"CAKE_PATHS_TOOLS", $"{Paths.Temp}/tools"},
    			{"CAKE_PATHS_MODULES", $"{Paths.Temp}/tools/Modules"}
    		}});
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Core.Scripting.AddinDirective")
    .IsDependentOn("Cake.Core.Scripting.AddinDirective.LoadNetStandardAddin");