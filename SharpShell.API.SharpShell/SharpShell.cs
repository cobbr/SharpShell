// Author: Ryan Cobb (@cobbr_io)
// Project: SharpShell (https://github.com/cobbr/SharpShell)
// License: BSD 3-Clause

using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace SharpShell.API.SharpShell
{
    class SharpShell
    {
        public static string WrapperFunctionFormat = 
@"using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Security;
using System.Security.Principal;
using System.Collections.Generic;
{0}

using SharpSploit.Credentials;
using SharpSploit.Enumeration;
using SharpSploit.Execution;
using SharpSploit.Generic;
using SharpSploit.Misc;

public static class {1}
{{
    public static object Execute()
    {{
        {2}
    }}
}}
";

        static void Main(string[] args)
        {
            bool printPrompt = true;
            List<string> UsingImports = new List<string>();
            List<string> lines = new List<string>();
            APICompiler compiler = new APICompiler(args.Length > 0 ? args[0] : "http://localhost:5000/api/SharpShell/compile");
            while (true)
            {
                // Display Prompt
                if (printPrompt)
                {
                    Console.Write("SharpShell > ");
                }
                else
                {
                    Console.Write(">>> ");
                }

                // Read Input
                string line = Console.ReadLine();

                // SharpShell Special Commands
                if (line.Trim().ToLower() == "exit" || line.Trim().ToLower() == "quit")
                {
                    return;
                }
                else if (line.Trim() == "")
                {
                    continue;
                }
                else if (line.Trim().EndsWith("\\"))
                {
                    printPrompt = false;
                    lines.Add(line.TrimEnd('\\'));
                    continue;
                }
                else if (line.Trim().StartsWith("using ") && line.Trim().Split(' ').Length == 2 && line.Trim().EndsWith(";"))
                {
                    Console.WriteLine("Import:\"" + line.Trim() + "\" now being used.");
                    UsingImports.Add(line.Trim());
                    continue;
                }

                try
                {
                    // Concatenation
                    printPrompt = true;
                    string source = String.Join(Environment.NewLine, lines.ToArray());
                    lines.Clear();
                    source = source + Environment.NewLine + line;
                    if (!source.Contains("return "))
                    {
                        source = "return " + source;
                    }

                    // Compilation
                    string ClassName = RandomString();
                    byte[] assemblyBytes = compiler.Compile(new APICompiler.CompilationRequest
                    {
                        Source = String.Format(WrapperFunctionFormat, String.Join(Environment.NewLine, UsingImports.ToArray()), ClassName, source),
                        AssemblyName = "SharpShell",
                        Optimize = true,
                        TargetDotNetVersion = APICompiler.DotNetVersion.Net35,
                        OutputKind = APICompiler.OutputKind.DynamicallyLinkedLibrary,
                        Platform = APICompiler.Platform.AnyCpu
                    });

                    // Execution
                    Assembly assembly = Assembly.Load(assemblyBytes);
                    object result = assembly.GetType(ClassName).GetMethod("Execute").Invoke(null, null);
                    Console.WriteLine(result.ToString());
                }
                catch (CompilationException e)
                {
                    Console.Error.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("RuntimeException: " + e.Message + e.StackTrace);
                }
            }
        }

        private static Random random = new Random();
        private static string RandomString()
        {
            const string alphachars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return alphachars[random.Next(alphachars.Length)] + new string(Enumerable.Repeat(chars, random.Next(10, 30)).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
