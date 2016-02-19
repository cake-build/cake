// Imports
#l "ensure.cake"

// References
#r "System.Runtime"

// Addins
#addin "nuget:?package=xunit.assert&version=2.1.0"

// Import namespaces.
using Xunit

public void RunTests()
{
	foreach(var task in Tasks)
	{
		try
		{
			task.Execute(Context);
		}
		catch(Exception ex)
		{
      var message = string.Format("{0}£{1}", task.Name, ex.Message);
			throw new CakeException(message);
		}
	}
}