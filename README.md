#Cake

Cake (C# Make) is a build automation system inspired by [Fake](http://fsharp.github.io/FAKE/).

##Example

###Using Cake from ScriptCs

```CSharp
var cake = Require<CakeScript>();

var configuration = Env.ScriptArgs.Count > 0 ? Env.ScriptArgs[0] : null;
if(configuration == null)
{
	configuration = "Debug";
}

// Task: Show welcome message.
cake.Task("Hello").Does(() =>
{
	Console.ForegroundColor = ConsoleColor.Yellow;
	Console.WriteLine("Welcome!");	
	Console.ResetColor();
});

// Task: Build the solution.
cake.Task("Build")
   .IsDependentOn("Hello")
   .Does(() =>
{
	cake.MSBuild("./src/Cake.sln", settings => 
		settings.WithProperty("Magic","1")
			.WithTarget("Build")
			.SetConfiguration(configuration)
		);
});

// Task: Run xUnit unit tests.
cake.Task("Run-Unit-Tests")
   .IsDependentOn("Build")
   .Does(() =>
{
	cake.XUnit(
		cake.GetFiles("./src/**/bin/" + configuration + "/*.Tests.dll"));
});

// Run the script.
cake.Run("Run-Unit-Tests");
```