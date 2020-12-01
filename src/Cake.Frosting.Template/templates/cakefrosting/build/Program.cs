using Cake.Core;
using Cake.Frosting;

public static class Program
{
    public static int Main(string[] args)
        => new CakeHost()
            .UseStartup<Program>()
            .Run(args);
}

public class Settings : FrostingContext
{
    public bool Delay { get; set; }

    public Settings(ICakeContext context)
        : base(context)
    {
        Delay = context.Arguments.HasArgument("delay");
    }
}

[TaskName("Hello")]
public sealed class HelloTask : FrostingTask<Settings>
{
    public override void Run(Settings context)
    {
        context.Log.Information("Hello");
    }
}

[TaskName("World")]
[IsDependentOn(typeof(HelloTask))]
public sealed class WorldTask : AsyncFrostingTask<Settings>
{
    // Tasks can be asynchronous
    public override async Task RunAsync(Settings context)
    {
        if (context.Delay)
        {
            context.Log.Information("Waiting...");
            await Task.Delay(1500);
        }

        context.Log.Information("World");
    }
}

[TaskName("Default")]
[IsDependentOn(typeof(WorldTask))]
public class DefaultTask : FrostingTask
{
}