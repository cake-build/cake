using Cake.Common.Diagnostics;
using Cake.Frosting;

[TaskName("Hello")]
public sealed class Hello : FrostingTask<Context>
{
    public override void Run(Context context)
    {
        context.Information("Hello World");
    }
}