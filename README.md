#Cake

Cake (C# Make) is a build automation system inspired by [Fake](http://fsharp.github.io/FAKE/).

##Example

###Using Cake from ScriptCs

```CSharp
var cake = Require<CakeScript>();

var shouldRunBuild = new Random(DateTime.Now.Millisecond).Next() % 2 == 0;

// Define a task
cake.Task("Hello").Does(c =>
{
	Console.ForegroundColor = ConsoleColor.Yellow;
	Console.WriteLine("Hello World!");	
	Console.ResetColor();
});

// Define the build task.
cake.Task("Build")
   .IsDependentOn("Hello")
   .WithCriteria(c => shouldRunBuild)
   .Does(c =>
{
	c.MSBuild("./src/Lunt.sln", settings => 
		settings.WithParameter("Magic","1")
			.WithTarget("Build")
			.SetConfiguration("Debug")
		);
});

// Run the build
cake.Run("Build");
```