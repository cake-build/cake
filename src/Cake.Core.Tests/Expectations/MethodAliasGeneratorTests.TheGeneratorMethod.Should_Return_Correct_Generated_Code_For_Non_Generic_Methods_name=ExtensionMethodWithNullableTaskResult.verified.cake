#nullable enable
[System.Diagnostics.DebuggerStepThrough]
public void NonGeneric_ExtensionMethodWithNullableTaskResult(System.Threading.Tasks.Task<System.String?> task)
    => Cake.Core.Tests.Data.MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNullableTaskResult(Context, task);

#nullable restore