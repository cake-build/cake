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
});


//////////////////////////////////////////////////////////////////////////////

Task("Cake.Core.Scripting.DefineDirective")
    .IsDependentOn("Cake.Core.Scripting.DefineDirective.Defined")
    .IsDependentOn("Cake.Core.Scripting.DefineDirective.NotDefined")
    .IsDependentOn("Cake.Core.Scripting.DefineDirective.Runtime")
    .IsDependentOn("Cake.Core.Scripting.DefineDirective.Cake");