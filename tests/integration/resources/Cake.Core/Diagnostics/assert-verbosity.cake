Task("Default")
    .Does(() =>
{
    var expected = Argument<string>("expected");
    if (!string.Equals(Context.Log.Verbosity.ToString(), expected, StringComparison.OrdinalIgnoreCase))
    {
        throw new Exception($"Expected verbosity '{expected}' but was '{Context.Log.Verbosity}'.");
    }
});
