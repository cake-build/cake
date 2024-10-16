// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;
using Cake.Frosting;
using Cake.Frosting.TaskChains;

public static class ChainedTasksProgram
{
    public static int MainDemo(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .UseChainedTaskConfigurator<AppTaskChainProvider>()
            .Run(args);
    }
}

class AppTaskChainProvider : ITaskChainProvider
{
    public TaskChainItem GetChain()
    {
        return Chain
            .Task<Task1>()
            .Group("Do something", _ =>
            {
                 // Reference the task type.
                _.Task<Task2>()
                 // Reference the task by name ([TaskName("Task3: do something")]).
                 .Task("Task3: do something");
            })
        // Task chain inline groups example.
        // .Group("Do something else", _ =>
        // {
        //     _.Task<SomeTask>()
        //      .Group("Internal group", _ =>
        //         {
        //             _.Task<OtherTask>();
        //         })
        //      .Task("Task name");
        // })
            // The last task in the chain which the Default task will be dependent on to execute all the chain.
            .Task<FinalTask>();
    }
}

[TaskName("Task1 chained")]
public sealed class Task1 : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information("Task1");
    }
}

public sealed class Task2 : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information("Task2");
    }
}

[TaskName("Task3: do something")]
public sealed class Task3 : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information("Task3");
    }
}

public sealed class FinalTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information("FinalTask");
    }
}

[TaskName("Default")]
public class ChainedDefaultTask : FrostingTask
{
}