#addin "nuget:?package=xunit.assert&version=2.4.1"

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