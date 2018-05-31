public sealed class ScriptContext
{
    public bool Initialized { get; }

    public ScriptContext(bool initialized)
    {
        Initialized = initialized;
    }
}

public sealed class AlternativeScriptContext
{
    public bool EnginesStarted { get; }

    public AlternativeScriptContext(bool initialized)
    {
        EnginesStarted = initialized;
    }
}