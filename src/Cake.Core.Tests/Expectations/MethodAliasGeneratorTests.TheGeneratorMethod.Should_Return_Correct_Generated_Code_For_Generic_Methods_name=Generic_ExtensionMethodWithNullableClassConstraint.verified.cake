#nullable enable
[System.Diagnostics.DebuggerStepThrough]
public void Generic_ExtensionMethodWithNullableClassConstraint<TTest>(TTest value)
where TTest : class?
    => Cake.Core.Tests.Data.MethodAliasGeneratorData.Generic_ExtensionMethodWithNullableClassConstraint<TTest>(Context, value);

#nullable restore