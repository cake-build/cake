#load "WoodpeckerCIProvider.cake"

Task("Cake.Common.Build.WoodpeckerCIProvider")
    .Does(() => {
        // Test WoodpeckerCI provider detection
        Information("WoodpeckerCI Provider:");
        Information("  IsRunningOnWoodpeckerCI: {0}", BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI);
        
        if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        {
            Information("  Environment:");
            Information("    CI: {0}", BuildSystem.WoodpeckerCI.Environment.CI);
            Information("    Workspace: {0}", BuildSystem.WoodpeckerCI.Environment.Workspace);
            
            Information("  Repository:");
            Information("    Repo: {0}", BuildSystem.WoodpeckerCI.Environment.Repository.Repo);
            Information("    Owner: {0}", BuildSystem.WoodpeckerCI.Environment.Repository.RepoOwner);
            Information("    Name: {0}", BuildSystem.WoodpeckerCI.Environment.Repository.RepoName);
            Information("    RepoUrl: {0}", BuildSystem.WoodpeckerCI.Environment.Repository.RepoUrl);
            Information("    RepoCloneUrl: {0}", BuildSystem.WoodpeckerCI.Environment.Repository.RepoCloneUrl);
            Information("    RepoCloneSshUrl: {0}", BuildSystem.WoodpeckerCI.Environment.Repository.RepoCloneSshUrl);
            Information("    DefaultBranch: {0}", BuildSystem.WoodpeckerCI.Environment.Repository.RepoDefaultBranch);
            Information("    Private: {0}", BuildSystem.WoodpeckerCI.Environment.Repository.RepoPrivate);
            Information("    TrustedNetwork: {0}", BuildSystem.WoodpeckerCI.Environment.Repository.RepoTrustedNetwork);
            Information("    TrustedVolumes: {0}", BuildSystem.WoodpeckerCI.Environment.Repository.RepoTrustedVolumes);
            Information("    TrustedSecurity: {0}", BuildSystem.WoodpeckerCI.Environment.Repository.RepoTrustedSecurity);
            
            Information("  Commit:");
            Information("    Sha: {0}", BuildSystem.WoodpeckerCI.Environment.Commit.Sha);
            Information("    Branch: {0}", BuildSystem.WoodpeckerCI.Environment.Commit.Branch);
            Information("    Message: {0}", BuildSystem.WoodpeckerCI.Environment.Commit.Message);
            Information("    Author: {0}", BuildSystem.WoodpeckerCI.Environment.Commit.Author);
            Information("    AuthorEmail: {0}", BuildSystem.WoodpeckerCI.Environment.Commit.AuthorEmail);
            
            Information("  Pipeline:");
            Information("    Number: {0}", BuildSystem.WoodpeckerCI.Environment.Pipeline.Number);
            Information("    Parent: {0}", BuildSystem.WoodpeckerCI.Environment.Pipeline.Parent);
            Information("    Event: {0}", BuildSystem.WoodpeckerCI.Environment.Pipeline.Event);
            Information("    Url: {0}", BuildSystem.WoodpeckerCI.Environment.Pipeline.Url);
            Information("    ForgeUrl: {0}", BuildSystem.WoodpeckerCI.Environment.Pipeline.ForgeUrl);
            Information("    DeployTarget: {0}", BuildSystem.WoodpeckerCI.Environment.Pipeline.DeployTarget);
            Information("    DeployTask: {0}", BuildSystem.WoodpeckerCI.Environment.Pipeline.DeployTask);
            Information("    Created: {0}", BuildSystem.WoodpeckerCI.Environment.Pipeline.Created);
            Information("    Started: {0}", BuildSystem.WoodpeckerCI.Environment.Pipeline.Started);
            Information("    Files: {0}", BuildSystem.WoodpeckerCI.Environment.Pipeline.Files);
            Information("    Author: {0}", BuildSystem.WoodpeckerCI.Environment.Pipeline.Author);
            Information("    Avatar: {0}", BuildSystem.WoodpeckerCI.Environment.Pipeline.Avatar);
            
            Information("  Workflow:");
            Information("    Name: {0}", BuildSystem.WoodpeckerCI.Environment.Workflow.Name);
            
            Information("  Step:");
            Information("    Name: {0}", BuildSystem.WoodpeckerCI.Environment.Step.Name);
            Information("    Number: {0}", BuildSystem.WoodpeckerCI.Environment.Step.Number);
            Information("    Started: {0}", BuildSystem.WoodpeckerCI.Environment.Step.Started);
            Information("    Url: {0}", BuildSystem.WoodpeckerCI.Environment.Step.Url);
            
            Information("  System:");
            Information("    Name: {0}", BuildSystem.WoodpeckerCI.Environment.System.Name);
            Information("    Version: {0}", BuildSystem.WoodpeckerCI.Environment.System.Version);
            Information("    Host: {0}", BuildSystem.WoodpeckerCI.Environment.System.Host);
            
            Information("  Forge:");
            Information("    Type: {0}", BuildSystem.WoodpeckerCI.Environment.Forge.Type);
            Information("    Url: {0}", BuildSystem.WoodpeckerCI.Environment.Forge.Url);
            
            // Test Commands
            Information("  Commands:");
            try
            {
                BuildSystem.WoodpeckerCI.Commands.SetEnvironmentVariable("TEST_VAR", "test_value");
                var retrievedValue = BuildSystem.WoodpeckerCI.Commands.GetEnvironmentVariable("TEST_VAR");
                Information("    SetEnvironmentVariable/GetEnvironmentVariable: {0}", retrievedValue);
            }
            catch (Exception ex)
            {
                Information("    Commands test failed: {0}", ex.Message);
            }
        }
        else
        {
            Information("  Not running on WoodpeckerCI");
        }
    });
