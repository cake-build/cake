public sealed class ScriptContext
{
    public bool Initialized { get; }

    public ScriptContext(bool initialized)
    {
        Initialized = initialized;
    }
}