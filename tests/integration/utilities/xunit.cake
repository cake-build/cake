#addin "nuget:?package=xunit.assert&version=2.2.0-beta2-build3300&prerelease"

// Usings
using Xunit;

public class Record
{
        public static Exception Exception(Action action)
        {
           if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            try
            {
                action();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
}