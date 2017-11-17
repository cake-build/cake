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

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Core.Scripting.DefineDirective")
    .IsDependentOn("Cake.Core.Scripting.DefineDirective.Defined")
    .IsDependentOn("Cake.Core.Scripting.DefineDirective.NotDefined");