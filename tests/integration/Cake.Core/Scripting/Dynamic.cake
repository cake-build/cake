using System.Dynamic;

Task("Cake.Core.Scripting.Dynamic")
    .Does(() =>
{
    dynamic result = new ExpandoObject();
    result.Result = true;

    Assert.True(result.Result);
});