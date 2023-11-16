public System.Int32 NonCached_Obsolete_ImplicitWarning_WithMessage
{
    [System.Diagnostics.DebuggerStepThrough]
    get
    {
        Context.Log.Warning("Warning: The alias NonCached_Obsolete_ImplicitWarning_WithMessage has been made obsolete. Please use Foo.Bar instead.");
        return Cake.Core.Tests.Data.PropertyAliasGeneratorData.NonCached_Obsolete_ImplicitWarning_WithMessage(Context);
    }
}