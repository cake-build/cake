public class BuildVersion
{
    public string Version { get; private set; }
    public string SemVersion { get; private set; }
    public string DotNetAsterix { get; private set; }
    public string Milestone { get; private set; }
    public string CakeVersion { get; private set; }

    public static BuildVersion Calculate(ICakeContext context, BuildParameters parameters)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        string version = null;
        string semVersion = null;
        string milestone = null;

        if (context.IsRunningOnWindows() && !parameters.SkipGitVersion)
        {
            context.Information("Calculating Semantic Version");
            if (!parameters.IsLocalBuild || parameters.IsPublishBuild || parameters.IsReleaseBuild)
            {
                context.GitVersion(new GitVersionSettings{
                    UpdateAssemblyInfoFilePath = "./src/SolutionInfo.cs",
                    UpdateAssemblyInfo = true,
                    OutputType = GitVersionOutput.BuildServer
                });

                version = context.EnvironmentVariable("GitVersion_MajorMinorPatch");
                semVersion = context.EnvironmentVariable("GitVersion_LegacySemVerPadded");
                milestone = string.Concat("v", version);
            }

            GitVersion assertedVersions = context.GitVersion(new GitVersionSettings
            {
                OutputType = GitVersionOutput.Json,
            });

            version = assertedVersions.MajorMinorPatch;
            semVersion = assertedVersions.LegacySemVerPadded;
            milestone = string.Concat("v", version);

            context.Information("Calculated Semantic Version: {0}", semVersion);
        }

        if (string.IsNullOrEmpty(version) || string.IsNullOrEmpty(semVersion))
        {
            context.Information("Fetching verson from first project.json...");
            version = ReadProjectJsonVersion(context);
            semVersion = version;
            milestone = string.Concat("v", version);
        }

        var cakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString();

        return new BuildVersion
        {
            Version = version,
            SemVersion = semVersion,
            DotNetAsterix = semVersion.Substring(version.Length).TrimStart('-'),
            Milestone = milestone,
            CakeVersion = cakeVersion
        };
    }

    public static string ReadProjectJsonVersion(ICakeContext context)
    {
        var projects = context.GetFiles("./**/project.json");
        foreach(var project in projects)
        {
            var content = System.IO.File.ReadAllText(project.FullPath, Encoding.UTF8);
            var node = Newtonsoft.Json.Linq.JObject.Parse(content);
            if(node["version"] != null)
            {
                var version = node["version"].ToString();
                return version.Replace("-*", "");
            }
        }
        throw new CakeException("Could not parse version.");
    }

    public bool PatchProjectJson(FilePath project)
    {
        var content = System.IO.File.ReadAllText(project.FullPath, Encoding.UTF8);
        var node = Newtonsoft.Json.Linq.JObject.Parse(content);
        if(node["version"] != null)
        {
            node["version"].Replace(string.Concat(Version, "-*"));
            System.IO.File.WriteAllText(project.FullPath, node.ToString(), Encoding.UTF8);
            return true;
        };
        return false;
    }
}