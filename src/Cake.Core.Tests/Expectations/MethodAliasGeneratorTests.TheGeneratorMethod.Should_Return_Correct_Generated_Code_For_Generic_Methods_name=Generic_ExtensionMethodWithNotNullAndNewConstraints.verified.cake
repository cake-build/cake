#nullable enable
[System.Diagnostics.DebuggerStepThrough]
public void Generic_ExtensionMethodWithNotNullAndNewConstraints<TTest>(TTest value)
where TTest : notnull, new()
    => Cake.Core.Tests.Data.MethodAliasGeneratorData.Generic_ExtensionMethodWithNotNullAndNewConstraints<TTest>(Context, value);

#nullable restore