#addin "nuget:?package=xunit.assert&version=2.2.0-beta2-build3300&prerelease"

// Usings
using Xunit;

public void AssertNotNull(object obj, string message)
{
   if (obj == null)
   {
      throw new CakeException(message);
   }
}