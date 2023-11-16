private System.Int32? _Cached_Obsolete_ImplicitWarning_WithMessage;
[Obsolete("Please use Foo.Bar instead.")]
public System.Int32 Cached_Obsolete_ImplicitWarning_WithMessage
{
    [System.Diagnostics.DebuggerStepThrough]
    get
    {
        Context.Log.Warning("Warning: The alias Cached_Obsolete_ImplicitWarning_WithMessage has been made obsolete. Please use Foo.Bar instead.");
        if (_Cached_Obsolete_ImplicitWarning_WithMessage==null)
        {
#pragma warning disable CS0618
            _Cached_Obsolete_ImplicitWarning_WithMessage = Cake.Core.Tests.Data.PropertyAliasGeneratorData.Cached_Obsolete_ImplicitWarning_WithMessage(Context);
#pragma warning restore CS0618
        }
        return _Cached_Obsolete_ImplicitWarning_WithMessage.Value;
    }
}