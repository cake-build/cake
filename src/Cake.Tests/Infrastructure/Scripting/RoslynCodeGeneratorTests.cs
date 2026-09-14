using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Scripting;
using Cake.Infrastructure.Scripting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Xunit;

namespace Cake.Tests.Infrastructure.Scripting
{
    public sealed class RoslynCodeGeneratorTests
    {
        [Theory]
        [InlineData(nameof(NullableAliasCompileGuardData.ExtensionMethodWithNullableParameter), true, "public void ExtensionMethodWithNullableParameter(System.String? parameter)")]
        [InlineData(nameof(NullableAliasCompileGuardData.ExtensionMethodWithNotNullConstraint), true, "where T : notnull")]
        [InlineData(nameof(NullableAliasCompileGuardData.ExtensionMethodWithNotNullAndNewConstraints), true, "where T : notnull, new()")]
        [InlineData(nameof(NullableAliasCompileGuardData.ExtensionMethodWithNullableTypeParameter), true, "public void ExtensionMethodWithNullableTypeParameter<T>(T? value)")]
        [InlineData(nameof(NullableAliasCompileGuardData.ExtensionMethodWithNullableTypeParameterArgument), true, "(System.Collections.Generic.IList<T?> values)")]
        [InlineData(nameof(NullableAliasCompileGuardData.ExtensionMethodWithUnconstrainedTypeParameter), false, "public void ExtensionMethodWithUnconstrainedTypeParameter<T>(T value)")]
        [InlineData(nameof(NullableAliasCompileGuardData.ExtensionMethodWithUnconstrainedTypeParameterArgument), false, "(System.Collections.Generic.IList<T> values)")]
        public void Should_Generate_Alias_Which_Compiles_Without_Diagnostics(string aliasName, bool expectNullableContext, string expectedSignature)
        {
            // Given / When
            var (code, diagnostics) = Compile(aliasName);

            // Then
            Assert.Contains(expectedSignature, code);
            Assert.Equal(expectNullableContext, code.Contains("#nullable enable"));

            // any warning at all, not just CS8632/CS8714, means the generated alias doesn't match
            // the nullability of the aliased method
            Assert.Empty(diagnostics
                .Where(diagnostic => diagnostic.Severity >= DiagnosticSeverity.Warning)
                .Select(diagnostic => $"{diagnostic.Id}: {diagnostic.GetMessage()}"));
        }

        private static (string Code, IEnumerable<Diagnostic> Diagnostics) Compile(string aliasName)
        {
            var method = typeof(NullableAliasCompileGuardData).GetMethod(aliasName);
            var alias = new ScriptAlias(method, ScriptAliasType.Method, new HashSet<string>());
            var script = new Cake.Core.Scripting.Script(
                Array.Empty<string>(),
                new[] { "var compiled = true;" },
                new[] { alias },
                Array.Empty<string>(),
                Array.Empty<string>(),
                Array.Empty<string>());
            var generator = new RoslynCodeGenerator();
            var code = generator.Generate(script);

            var options = ScriptOptions.Default
                .AddReferences(typeof(ICakeContext).Assembly)
                .AddReferences(typeof(NullableAliasCompileGuardData).Assembly);
            var roslynScript = CSharpScript.Create(code, options, typeof(NullableAliasScriptHost));

            return (code, roslynScript.GetCompilation().GetDiagnostics(TestContext.Current.CancellationToken));
        }

        public sealed class NullableAliasScriptHost
        {
            public ICakeContext Context { get; set; }
        }
    }

    public static class NullableAliasCompileGuardData
    {
        [CakeMethodAlias]
#nullable enable
        public static void ExtensionMethodWithNullableParameter(this ICakeContext context, string? parameter)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void ExtensionMethodWithNotNullConstraint<T>(this ICakeContext context, T value)
            where T : notnull
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void ExtensionMethodWithNotNullAndNewConstraints<T>(this ICakeContext context, T value)
            where T : notnull, new()
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void ExtensionMethodWithNullableTypeParameter<T>(this ICakeContext context, T? value)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void ExtensionMethodWithNullableTypeParameterArgument<T>(this ICakeContext context, IList<T?> values)
            where T : class
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void ExtensionMethodWithUnconstrainedTypeParameter<T>(this ICakeContext context, T value)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void ExtensionMethodWithUnconstrainedTypeParameterArgument<T>(this ICakeContext context, IList<T> values)
#nullable disable
        {
            throw new NotImplementedException();
        }
    }
}
