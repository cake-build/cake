using Cake.Common;
using Cake.Core.Diagnostics;
using Cake.Frosting;
using Xunit;

[TaskName("Verbosity")]
public sealed class VerbosityTask : FrostingTask<Context>
{
    public override void Run(Context context)
    {
        var expected = context.Argument<string>("expected");
        Assert.Equal(expected, context.Log.Verbosity.ToString());
    }
}
