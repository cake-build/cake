#define FOO
#load "./../../utilities/xunit.cake"


Task("Cake.Core.Scripting.DefineDirective.Defined")
    .Does(() =>
{
    bool foo;
#if (FOO)
    foo = true;
#else
    foo = false;
#endif
    Assert.True(foo);
});

Task("Cake.Core.Scripting.DefineDirective.NotDefined")
    .Does(() =>
{
    bool bar;
#if (BAR)
    bar = true;
#else
    bar = false;
#endif
    Assert.False(bar);
});

Task("Cake.Core.Scripting.DefineDirective.Runtime")
    .Does(context =>
{
#if NETFRAMEWORK
                Assert.Equal(".NETFramework,Version=v4.6.1",
#elif !NETCOREAPP
                Assert.Equal(".NETStandard,Version=v2.0",
#else
                Assert.Equal(".NETCoreApp,Version=v" +
#endif
#if NETCOREAPP2_0
                    "2.0",
#elif NETCOREAPP2_1
                    "2.1",
#elif NETCOREAPP2_2
                    "2.2",
#elif NETCOREAPP3_0
                    "3.0",
#elif NETCOREAPP3_1
                    "3.1",
#elif NET5_0
                    "5.0",
#elif NET6_0
                    "6.0",
#elif NET7_0
                    "7.0",
#elif NET8_0
                    "8.0",
#elif NET9_0
                    "9.0",
#elif NET10_0
                    "10.0",
#elif NET11_0
                    "11.0",
#endif
                    context.Environment.Runtime.BuiltFramework.FullName);
});

Task("Cake.Core.Scripting.DefineDirective.Cake")
    .Does(() =>
{
    bool cake;
#if (CAKE)
    cake = true;
#else
    cake = false;
#endif
    Assert.True(cake);

    bool cake7;
#if (CAKE_7)
    cake7 = true;
#else
    cake7 = false;
#endif
    Assert.True(cake7);

    bool cake7OrGreater;
#if (CAKE_7_OR_GREATER)
    cake7OrGreater = true;
#else
    cake7OrGreater = false;
#endif
    Assert.True(cake7OrGreater);
});

#if NET5_0_OR_GREATER
Task("Cake.Core.Scripting.DefineDirective.C#9")
    .Does(() =>
{
    // given
    var csharpNine = new CSharpNine(true);
    Assert.True(csharpNine.IsNine);
});

public record CSharpNine(bool IsNine);
#endif

#if NET6_0_OR_GREATER
Task("Cake.Core.Scripting.DefineDirective.C#10")
    .Does(() =>
{
    // Given
    const string world = "world";
    const string hello = "Hello";

    // When
    const string helloWorld = $"{hello} {world}!";

    // Then
    Assert.Equal("Hello world!", helloWorld);
});
#endif

#if NET7_0_OR_GREATER
Task("Cake.Core.Scripting.DefineDirective.C#11")
    .Does(() =>
{
    // Given / When / Then
    const string longMessage = """
    This is a long message.
    It has several lines.
        Some are indented
                more than others.
    Some should start at the first column.
    Some have "quoted text" in them.
    """;
});
#endif

#if NET8_0_OR_GREATER
Task("Cake.Core.Scripting.DefineDirective.C#12")
    .Does(() =>
{
    // Given / When / Then
    int[] row0 = [1, 2, 3];
    int[] row1 = [4, 5, 6];
    int[] row2 = [7, 8, 9];
    int[] single = [..row0, ..row1, ..row2];
});
#endif

#if NET9_0_OR_GREATER
Task("Cake.Core.Scripting.DefineDirective.C#13")
    .Does(() =>
{
    // Given
    string Concat(params ReadOnlySpan<string> items)
        => $"\e[31m{items[^1]}\e[31m{items[^2]}";
    var concat = Concat;

    // When
    var result = concat("World", "Hello");

    // Then
    Assert.Equal("\e[31mHello\e[31mWorld", result);
});
#endif

#if NET10_0_OR_GREATER
Task("Cake.Core.Scripting.DefineDirective.C#14")
    .Does(() =>
{
    // Given - Test C# 14 unbound generic types with nameof
    var listName = nameof(List<>);
    var dictionaryName = nameof(Dictionary<,>);
    var tupleName = nameof(Tuple<,,>);

    // When & Then
    Assert.Equal("List", listName);
    Assert.Equal("Dictionary", dictionaryName);
    Assert.Equal("Tuple", tupleName);
});
#endif

#if NET11_0_OR_GREATER
Task("Cake.Core.Scripting.DefineDirective.C#15")
    .Does(() =>
{
    // Given - Test C# 15 labeled break
    var n = 0;

    // When
    outer: for (var i = 0; i < 3; i++)
    {
        for (var j = 0; j < 3; j++)
        {
            n++;
            if (j == 1)
            {
                break outer;
            }
        }
    }

    // Then
    Assert.Equal(2, n);
});
#endif

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Core.Scripting.DefineDirective")
#if NET5_0_OR_GREATER
    .IsDependentOn("Cake.Core.Scripting.DefineDirective.C#9")
#endif
#if NET6_0_OR_GREATER
    .IsDependentOn("Cake.Core.Scripting.DefineDirective.C#10")
#endif
#if NET7_0_OR_GREATER
    .IsDependentOn("Cake.Core.Scripting.DefineDirective.C#11")
#endif
#if NET8_0_OR_GREATER
    .IsDependentOn("Cake.Core.Scripting.DefineDirective.C#12")
#endif
#if NET9_0_OR_GREATER
    .IsDependentOn("Cake.Core.Scripting.DefineDirective.C#13")
#endif
#if NET10_0_OR_GREATER
    .IsDependentOn("Cake.Core.Scripting.DefineDirective.C#14")
#endif
#if NET11_0_OR_GREATER
    .IsDependentOn("Cake.Core.Scripting.DefineDirective.C#15")
#endif
    .IsDependentOn("Cake.Core.Scripting.DefineDirective.Defined")
    .IsDependentOn("Cake.Core.Scripting.DefineDirective.NotDefined")
    .IsDependentOn("Cake.Core.Scripting.DefineDirective.Runtime")
    .IsDependentOn("Cake.Core.Scripting.DefineDirective.Cake");