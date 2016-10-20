#load "./../../../utilities/xunit.cake"
#load "./../../../utilities/paths.cake"
#load "./../../../utilities/comparers/ProjectParserResultEqualityComparer.cake"

Task("Cake.Common.Solution.Project.ProjectAliases.ParseProject")
    .Does(() =>
{
    // Given
    var path = Paths.Resources.Combine("./Cake.Common/Solution/Project");
    var file = path.CombineWithFilePath("./Project.csproj");
    var expect = new ProjectParserResult(
                   configuration: "Debug",
                   platform: "AnyCPU",
                   projectGuid: "{4147F50A-8743-4393-A414-EFC707D4BA2C}",
                   outputType: "Library",
                   outputPath: Directory("bin/Debug"),
                   rootNameSpace: "Project.RootNamespace",
                   assemblyName: "Project",
                   targetFrameworkVersion: "v4.5.2",
                   targetFrameworkProfile: null,
                   files: new [] {
                        new ProjectFile {
                            Compile = true,
                            FilePath = path.CombineWithFilePath("./Properties/AssemblyInfo.cs"),
                            RelativePath = "Properties\\AssemblyInfo.cs"
                        }
                   },
                   references: new [] {
                       new ProjectAssemblyReference { Include = "System" },
                       new ProjectAssemblyReference { Include = "System.Core" },
                       new ProjectAssemblyReference { Include = "System.Xml.Linq" },
                       new ProjectAssemblyReference { Include = "System.Data.DataSetExtensions" },
                       new ProjectAssemblyReference { Include = "Microsoft.CSharp" },
                       new ProjectAssemblyReference { Include = "System.Data" },
                       new ProjectAssemblyReference { Include = "System.Net.Http" },
                       new ProjectAssemblyReference { Include = "System.Xml" }
                   },
                   projectReferences: new [] {
                      new ProjectReference {
                        FilePath = path.CombineWithFilePath("../ReferenceProject/ReferenceProject.csproj"),
                        Name = "ReferenceProject",
                        Project = "{a1f3f53d-317b-451e-b616-de4c408bf740}",
                        RelativePath = "..\\ReferenceProject\\ReferenceProject.csproj"
                    }
                   }
                );

    // When
    var result = ParseProject(file);

    // Then
    Assert.NotNull(result);
    Assert.Equal(expect.Files, result.Files, ProjectFileEqualityComparer.Comparer);
    Assert.Equal(expect.References, result.References, ProjectAssemblyReferenceEqualityComparer.Comparer);
    Assert.Equal(expect.ProjectReferences, result.ProjectReferences, ProjectReferenceEqualityComparer.Comparer);
    Assert.Equal(expect, result, ProjectParserResultEqualityComparer.Comparer);
});


Task("Cake.Common.Solution.Project.ProjectAliases")
    .IsDependentOn("Cake.Common.Solution.Project.ProjectAliases.ParseProject");