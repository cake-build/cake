private System.Int32? _Cached_Obsolete_ImplicitWarning_NoMessage;
[Obsolete]
public System.Int32 Cached_Obsolete_ImplicitWarning_NoMessage
#pragma warning disable CS0618
    => _Cached_Obsolete_ImplicitWarning_NoMessage ??= Cake.Core.Tests.Data.PropertyAliasGeneratorData.Cached_Obsolete_ImplicitWarning_NoMessage(Context);
#pragma warning restore CS0618