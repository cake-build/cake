#load "./../../../../utilities/xunit.cake"
#load "./../../../../utilities/paths.cake"
#load "./../../../../utilities/io.cake"

Task("Cake.Common.Solution.Project.Properties.AssemblyInfoAliases.ParseAssemblyInfo")
    .Does(() =>
{
    // Given
    var path = Paths.Resources.Combine("./Cake.Common/Solution/Project/Properties");
    var file = path.CombineWithFilePath("./AssemblyInfo.cs");
    var expect = new  AssemblyInfoParseResult(
                       clsCompliant: "true",
                       company: "The Company",
                       comVisible: "false",
                       configuration: "The Configuration",
                       copyright: "The Copyright",
                       description: "The Description",
                       assemblyFileVersion: "1.0.1",
                       guid: "4147f50a-8743-4393-a414-efc707d4ba2c",
                       assemblyInformationalVersion: "1.0-@",
                       product: "The Product",
                       title: "The Title",
                       trademark: "The Trademark",
                       assemblyVersion: "1.0",
                       internalsVisibleTo: new [] {
                            "Eeny",
                            "Meeny",
                            "Miny",
                            "Moe",
                            "Spacey"
                       }
                );

    // When
    var result = ParseAssemblyInfo(file);

    // Then
    Assert.NotNull(result);
    Assert.Equal(expect.ClsCompliant, result.ClsCompliant);
    Assert.Equal(expect.Company, result.Company);
    Assert.Equal(expect.ComVisible, result.ComVisible);
    Assert.Equal(expect.Configuration, result.Configuration);
    Assert.Equal(expect.Copyright, result.Copyright);
    Assert.Equal(expect.Description, result.Description);
    Assert.Equal(expect.Guid, result.Guid);
    Assert.Equal(expect.AssemblyInformationalVersion, result.AssemblyInformationalVersion);
    Assert.Equal(expect.Product, result.Product);
    Assert.Equal(expect.Title, result.Title);
    Assert.Equal(expect.Trademark, result.Trademark);
    Assert.Equal(expect.AssemblyVersion, result.AssemblyVersion);
    Assert.Equal(expect.InternalsVisibleTo, result.InternalsVisibleTo);
});

Task("Cake.Common.Solution.Project.Properties.AssemblyInfoAliases.CreateAssemblyInfo")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Solution/Project/Properties");
    var file = path.CombineWithFilePath("SolutionInfo.cs");
    EnsureDirectoryExist(path);
    var settings = new AssemblyInfoSettings {
       CLSCompliant = true,
       Company = "The Company",
       ComVisible = false,
       Configuration = "The Configuration",
       Copyright = "The Copyright",
       Description = "The Description",
       FileVersion = "1.0.1",
       Guid = "4147f50a-8743-4393-a414-efc707d4ba2c",
       InformationalVersion = "1.0-@",
       Product = "The Product",
       Title = "The Title",
       Trademark = "The Trademark",
       Version = "1.0",
       InternalsVisibleTo = new [] {
            "Eeny",
            "Meeny",
            "Miny",
            "Moe"
       }
    };
    var expectFile = Paths.Resources.CombineWithFilePath("./Cake.Common/Solution/Project/Properties/SolutionInfo.cs");

    // When
    CreateAssemblyInfo(file, settings);

    // Then
    Assert.True(System.IO.File.Exists(file.FullPath));
    Assert.True(FileHashEquals(expectFile, file));
});

Task("Cake.Common.Solution.Project.Properties.AssemblyInfoAliases")
    .IsDependentOn("Cake.Common.Solution.Project.Properties.AssemblyInfoAliases.ParseAssemblyInfo")
    .IsDependentOn("Cake.Common.Solution.Project.Properties.AssemblyInfoAliases.CreateAssemblyInfo");