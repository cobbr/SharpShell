// Author: Ryan Cobb (@cobbr_io)
// Project: SharpShell (https://github.com/cobbr/SharpShell)
// License: BSD 3-Clause

using System.IO;
using System.Text;
using System.Reflection;

namespace SharpShell.API.Models
{
    public static class Common
    {
        public static Encoding SharpShellEncoding = Encoding.UTF8;
        public static string SharpShellDirectory = Assembly.GetExecutingAssembly().Location.Split("bin")[0].Split("SharpShell.API.dll")[0];
        public static string SharpShellDataDirectory = SharpShellDirectory + "Data" + Path.DirectorySeparatorChar;

        public static string SharpShellResourcesDirectory = SharpShellDataDirectory + "Resources" + Path.DirectorySeparatorChar;
        public static string SharpShellResourcesConfig = SharpShellResourcesDirectory + "resources.yml";

        public static string SharpShellReferencesDirectory = SharpShellDataDirectory + "References" + Path.DirectorySeparatorChar;
        public static string SharpShellReferencesConfig = SharpShellReferencesDirectory + "references.yml";

        public static string Net35Directory = SharpShellReferencesDirectory + "net35" + Path.DirectorySeparatorChar;
        public static string Net40Directory = SharpShellReferencesDirectory + "net40" + Path.DirectorySeparatorChar;

        public static string SharpShellSourceDirectory = SharpShellDataDirectory + "Source" + Path.DirectorySeparatorChar;
    }
}