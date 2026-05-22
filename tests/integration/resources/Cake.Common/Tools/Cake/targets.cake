#load "../../../../utilities/xunit.cake"

Setup(context => {
    Information($"Tasks to run: {string.Join(", ", context.TasksToExecute.Select(x => x.Name))}");
});

Setup(ctx => new List<string>());

Teardown<List<string>>((ctx, data)=>{
    var result = string.Join(',', data);
    var expected = ctx.Argument("expected", string.Empty);
    Assert.Equal(expected, result);
});

TaskTeardown<List<string>>((ctx, data) => data.Add(ctx.Task.Name));

Task("A")
    .Does(_=>{})
    .IsDependentOn("F");

Task("B")
    .Does(_=>{})
    .IsDependentOn("F");

Task("C")
    .Does(_=>{})
    .IsDependentOn("F");

Task("D")
    .Does(_=>{})
    .IsDependentOn("E");

Task("E")
    .Does(_=>{});

Task("F")
    .Does(_=>{});

Task("Default");

RunTargets(Arguments<string>("target", new []{ "Default" }))