Task("Cake.Core.Scripting.SystemCollections.ToDictionary")
    .Does(() =>
{
    var dictionary = Array.Empty<int>().ToDictionary(key => key, value=>value);
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Core.Scripting.SystemCollections")
    .IsDependentOn("Cake.Core.Scripting.SystemCollections.ToDictionary");