private System.Int32? _Cached_Obsolete_ExplicitWarning_WithMessage;
[Obsolete("Please use Foo.Bar instead.", false)]
public System.Int32 Cached_Obsolete_ExplicitWarning_WithMessage
#pragma warning disable CS0618
    => _Cached_Obsolete_ExplicitWarning_WithMessage ??= Cake.Core.Tests.Data.PropertyAliasGeneratorData.Cached_Obsolete_ExplicitWarning_WithMessage(Context);
#pragma warning restore CS0618