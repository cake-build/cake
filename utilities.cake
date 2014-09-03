public int GetBuildNumber()
{	
	var context = GetContext();
	var value = context.Environment.GetEnvironmentVariable("APPVEYOR_BUILD_NUMBER");
	if(value != null)
	{
		int version = 0;
		if(int.TryParse(value, out version))
		{			
			return version;
		}
	}	
	return 0;	
}

public void SetBuildVersion(string version)
{
	if(GetBuildSystemName() == "AppVeyor")
	{
		StartProcess("appveyor", new ProcessSettings {
			Arguments = string.Concat("UpdateBuild -Version \"", version, "\"")
		});
	}
}

public string GetBuildSystemName()
{
	var context = GetContext();
	if(context.Environment.GetEnvironmentVariable("APPVEYOR") != null)
	{
		return "AppVeyor";
	}
	return "Local";
}