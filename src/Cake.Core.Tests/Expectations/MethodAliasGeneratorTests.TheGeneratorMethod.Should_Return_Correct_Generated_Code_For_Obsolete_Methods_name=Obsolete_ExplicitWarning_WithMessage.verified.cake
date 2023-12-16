[System.Diagnostics.DebuggerStepThrough]
public void Obsolete_ExplicitWarning_WithMessage()
#pragma warning disable 0618
    => Cake.Core.Tests.Data.MethodAliasGeneratorData.Obsolete_ExplicitWarning_WithMessage(Context);
#pragma warning restore 0618