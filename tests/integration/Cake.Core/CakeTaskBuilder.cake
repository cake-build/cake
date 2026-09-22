#load "./../../utilities/xunit.cake"

using System.Collections.Generic;
using System.Linq;

Task("Cake.Core.CakeTaskBuilder.IsDependentOn.Names.A");
Task("Cake.Core.CakeTaskBuilder.IsDependentOn.Names.B");

var cakeCoreCakeTaskBuilderDependentOnNames = Task("Cake.Core.CakeTaskBuilder.IsDependentOn.Names")
    .IsDependentOn(["Cake.Core.CakeTaskBuilder.IsDependentOn.Names.A", "Cake.Core.CakeTaskBuilder.IsDependentOn.Names.B"]);

Assert.Equal(
    new[]
    {
        "Cake.Core.CakeTaskBuilder.IsDependentOn.Names.A",
        "Cake.Core.CakeTaskBuilder.IsDependentOn.Names.B"
    },
    cakeCoreCakeTaskBuilderDependentOnNames.Task.Dependencies.Select(dependency => dependency.Name));

var cakeCoreCakeTaskBuilderDependentOnListNames = new List<string>
{
    "Cake.Core.CakeTaskBuilder.IsDependentOn.List.A",
    "Cake.Core.CakeTaskBuilder.IsDependentOn.List.B"
};

Task("Cake.Core.CakeTaskBuilder.IsDependentOn.List.A");
Task("Cake.Core.CakeTaskBuilder.IsDependentOn.List.B");

var cakeCoreCakeTaskBuilderDependentOnList = Task("Cake.Core.CakeTaskBuilder.IsDependentOn.List")
    .IsDependentOn(cakeCoreCakeTaskBuilderDependentOnListNames);

Assert.Equal(
    cakeCoreCakeTaskBuilderDependentOnListNames,
    cakeCoreCakeTaskBuilderDependentOnList.Task.Dependencies.Select(dependency => dependency.Name));

var cakeCoreCakeTaskBuilderDependentOnBuilderA = Task("Cake.Core.CakeTaskBuilder.IsDependentOn.Builders.A");
var cakeCoreCakeTaskBuilderDependentOnBuilderB = Task("Cake.Core.CakeTaskBuilder.IsDependentOn.Builders.B");

var cakeCoreCakeTaskBuilderDependentOnBuilders = Task("Cake.Core.CakeTaskBuilder.IsDependentOn.Builders")
    .IsDependentOn([cakeCoreCakeTaskBuilderDependentOnBuilderA, cakeCoreCakeTaskBuilderDependentOnBuilderB]);

Assert.Equal(
    new[]
    {
        "Cake.Core.CakeTaskBuilder.IsDependentOn.Builders.A",
        "Cake.Core.CakeTaskBuilder.IsDependentOn.Builders.B"
    },
    cakeCoreCakeTaskBuilderDependentOnBuilders.Task.Dependencies.Select(dependency => dependency.Name));

Task("Cake.Core.CakeTaskBuilder.IsDependeeOf.Names.Parent1");
Task("Cake.Core.CakeTaskBuilder.IsDependeeOf.Names.Parent2");

var cakeCoreCakeTaskBuilderDependeeOfNames = Task("Cake.Core.CakeTaskBuilder.IsDependeeOf.Names")
    .IsDependeeOf(["Cake.Core.CakeTaskBuilder.IsDependeeOf.Names.Parent1", "Cake.Core.CakeTaskBuilder.IsDependeeOf.Names.Parent2"]);

Assert.Equal(
    new[]
    {
        "Cake.Core.CakeTaskBuilder.IsDependeeOf.Names.Parent1",
        "Cake.Core.CakeTaskBuilder.IsDependeeOf.Names.Parent2"
    },
    cakeCoreCakeTaskBuilderDependeeOfNames.Task.Dependees.Select(dependee => dependee.Name));

var cakeCoreCakeTaskBuilderDependeeOfBuilderParent1 = Task("Cake.Core.CakeTaskBuilder.IsDependeeOf.Builders.Parent1");
var cakeCoreCakeTaskBuilderDependeeOfBuilderParent2 = Task("Cake.Core.CakeTaskBuilder.IsDependeeOf.Builders.Parent2");

var cakeCoreCakeTaskBuilderDependeeOfBuilders = Task("Cake.Core.CakeTaskBuilder.IsDependeeOf.Builders")
    .IsDependeeOf([cakeCoreCakeTaskBuilderDependeeOfBuilderParent1, cakeCoreCakeTaskBuilderDependeeOfBuilderParent2]);

Assert.Equal(
    new[]
    {
        "Cake.Core.CakeTaskBuilder.IsDependeeOf.Builders.Parent1",
        "Cake.Core.CakeTaskBuilder.IsDependeeOf.Builders.Parent2"
    },
    cakeCoreCakeTaskBuilderDependeeOfBuilders.Task.Dependees.Select(dependee => dependee.Name));

Task("Cake.Core.CakeTaskBuilder")
    .IsDependentOn([
        "Cake.Core.CakeTaskBuilder.IsDependentOn.Names",
        "Cake.Core.CakeTaskBuilder.IsDependentOn.List",
        "Cake.Core.CakeTaskBuilder.IsDependentOn.Builders",
        "Cake.Core.CakeTaskBuilder.IsDependeeOf.Names",
        "Cake.Core.CakeTaskBuilder.IsDependeeOf.Builders"
    ]);
