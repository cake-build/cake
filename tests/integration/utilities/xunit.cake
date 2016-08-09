#addin "nuget:?package=xunit.assert"

// Usings
using Xunit;

public void AssertNotNull(object obj, string message)
{
   if (obj == null)
   {
      throw new CakeException(message);
   }
}