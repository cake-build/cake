#Cake

Cake (C# Make) is a build automation system inspired by [Fake](http://fsharp.github.io/FAKE/).

##Example

###1. Download Cake

```
C:\Project> NuGet.exe "install" "Cake" "-OutputDirectory" "Tools" "-ExcludeVersion"
```

###2. Create build script

```CSharp
var configuration = "Debug";

Task("Build")
	.Does(c =>
{
	// Build project using MSBuild
	c.MSBuild("./src/Cake.sln", settings => 
		settings.WithProperty("Magic","1")
			.WithTarget("Build")
			.SetConfiguration(configuration)
		);
});

Task("Run-Unit-Tests")
	.IsDependentOn("Build")
	.WithCriteria(() => DateTime.Now.Second % 2 == 0)
	.Does(c =>
{
	// Run unit tests.
	c.XUnit("./src/**/bin/" + configuration + "/*.Tests.dll");
});

// Run the script.
Run("Run-Unit-Tests");
```

###3. Run build script

```
C:\Project\Tools\Cake> Cake ../../build.csx