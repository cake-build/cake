using System;
using System.IO;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;

namespace Cake
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var file = args.Length == 1 ? args[0] : null;
            if (file == null)
            {
                return 1;
            }
            if (!File.Exists(file))
            {
                Console.WriteLine("Could not find script '{0}'.", file);
                return 1;
            }

            try
            {
                // Read the code from the file.
                var code = File.ReadAllText(file);

                var engine = CreateEngine();
                var script = new ScriptEngine();
                var session = CreateSession(script, engine);

                // Execute the script.
                return Execute(session, code);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unhandled error occured while executing the script.");
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        private static CakeEngine CreateEngine()
        {
            var fileSystem = new FileSystem();
            var environment = new CakeEnvironment();
            var log = new NullLog();
            var globber = new Globber(fileSystem, environment);
            return new CakeEngine(fileSystem, environment, log, globber);
        }

        private static Session CreateSession(ScriptEngine script, CakeEngine host)
        {
            var references = new[] { "System", "System.Core", "System.Data", 
                "System.Data.DataSetExtensions", "System.Xml", "System.Xml.Linq" };
            var namespaces = new[] { "System", "System.Collections.Generic", "System.Linq", 
                "System.Text", "System.Threading.Tasks", "System.IO" };

            var session = script.CreateSession(host);

            // Add references.
            session.AddReference(typeof(CakeEngine).Assembly);
            foreach (var reference in references)
            {
                session.AddReference(reference);
            }

            // Import namespaces.
            session.ImportNamespace("Cake.Core");
            foreach (var defaultNamespace in namespaces)
            {
                session.ImportNamespace(defaultNamespace);
            }

            return session;
        }

        private static int Execute(Session session, string code)
        {
            try
            {
                var submission = session.CompileSubmission<object>(code);
                try
                {
                    submission.Execute();
                    return 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occured while executing the script.");
                    Console.WriteLine(ex.Message);
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured while compiling the script.");
                Console.WriteLine(ex.Message);
                return 1;
            }
        }
    }
}
