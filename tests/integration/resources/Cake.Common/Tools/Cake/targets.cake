#load "../../../../utilities/xunit.cake"

Setup(ctx => new List<string>());

Teardown<List<string>>((ctx, data)=>{
    var result = string.Join(',', data);
    var expected = ctx.Argument("expected", string.Empty);
    Assert.Equal(expected, result);
});

TaskTeardown<List<string>>((ctx, data) => data.Add(ctx.Task.Name));

Task("A")
    .Does(_=>{});

Task("B")
    .Does(_=>{});

Task("C")
    .Does(_=>{});

Task("D")
    .Does(_=>{})
    .IsDependentOn("E");

Task("E")
    .Does(_=>{});

Task("Default");

RunTargets(Arguments<string>("target", new []{ "Default" }))