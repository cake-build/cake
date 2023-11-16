[System.Diagnostics.DebuggerStepThrough]
public void Obsolete_ImplicitWarning_WithMessage()
{
    Context.Log.Warning("Warning: The alias Obsolete_ImplicitWarning_WithMessage has been made obsolete. Please use Foo.Bar instead.");
#pragma warning disable 0618
    Cake.Core.Tests.Data.MethodAliasGeneratorData.Obsolete_ImplicitWarning_WithMessage(Context);
#pragma warning restore 0618
}