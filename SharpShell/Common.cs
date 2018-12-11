// Author: Ryan Cobb (@cobbr_io)
// Project: SharpShell (https://github.com/cobbr/SharpShell)
// License: BSD 3-Clause

using System.IO;
using System.Text;
using System.Reflection;

namespace SharpShell
{
    public static class Common
    {
        public static Encoding SharpShellEncoding = Encoding.UTF8;
        public static string SharpShellDirectory = Assembly.GetExecutingAssembly().Location.SplitFirst("bin");
        public static string SharpShellDataDirectory = SharpShellDirectory + "Data" + Path.DirectorySeparatorChar;

        public static string SharpShellResourcesDirectory = SharpShellDataDirectory + "Resources" + Path.DirectorySeparatorChar;
        public static string SharpShellResourcesConfig = SharpShellResourcesDirectory + "resources.yml";

        public static string SharpShellReferencesDirectory = SharpShellDataDirectory + "References" + Path.DirectorySeparatorChar;
        public static string SharpShellReferencesConfig = SharpShellReferencesDirectory + "references.yml";

        public static string Net35Directory = SharpShellReferencesDirectory + "net35" + Path.DirectorySeparatorChar;
        public static string Net40Directory = SharpShellReferencesDirectory + "net40" + Path.DirectorySeparatorChar;

        public static string SharpShellSourceDirectory = SharpShellDataDirectory + "Source" + Path.DirectorySeparatorChar;
    }

    public static class Utilities
    {
        public static string SplitFirst(this string original, string split)
        {
            int index = original.IndexOf(split);
            if (index == -1) { return original; }
            return original.Substring(0, index);
        }
    }
}