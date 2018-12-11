// Author: Ryan Cobb (@cobbr_io)
// Project: SharpShell (https://github.com/cobbr/SharpShell)
// License: BSD 3-Clause

using System;
using System.IO;
using System.Net;
using System.Text;
using System.IO.Compression;
using System.Collections.Generic;

namespace SharpShell.API.SharpShell
{
    public class APICompiler
    {
        public enum OutputKind
        {
            ConsoleApplication = 0,
            WindowsApplication = 1,
            DynamicallyLinkedLibrary = 2,
            NetModule = 3,
            WindowsRuntimeMetaData = 4,
            WindowsRuntimeApplication = 5
        }

        public enum DotNetVersion
        {
            Net35,
            Net40
        }

        public enum Platform
        {
            AnyCpu = 0,
            X86 = 1,
            X64 =2
        }

        public class EmbeddedResource
        {
            public string Name { get; set; }
            public string File { get; set; }
            public Platform Platform { get; set; } = Platform.AnyCpu;
            public bool Enabled { get; set; } = false;
        }

        public class Reference
        {
            public string File { get; set; }
            public DotNetVersion Framework { get; set; } = DotNetVersion.Net35;
            public bool Enabled { get; set; } = false;
        }

        public class CompilationRequest
        {
            public string Source { get; set; } = null;
            public string SourceDirectory { get; set; } = null;
            public string ResourceDirectory { get; set; } = null;
            public string ReferenceDirectory { get; set; } = null;

            public DotNetVersion TargetDotNetVersion { get; set; } = DotNetVersion.Net35;
            public OutputKind OutputKind { get; set; } = OutputKind.DynamicallyLinkedLibrary;
            public Platform Platform { get; set; } = Platform.AnyCpu;
            public bool Optimize = true;

            public string AssemblyName { get; set; } = null;
            public List<Reference> References { get; set; } = new List<Reference>();
            public List<EmbeddedResource> EmbeddedResources { get; set; } = new List<EmbeddedResource>();
        }


        private string SharpShellURI { get; set; } = "";
        private WebClient client { get; set; } = new WebClient();

        public APICompiler(string SharpShellURI = "http://localhost:5000/api/SharpShell/compile")
        {
            this.SharpShellURI = SharpShellURI;
        }

        public byte[] Compile(CompilationRequest compilationRequest)
        {
            this.SetHeaders();
            try
            {
                return Convert.FromBase64String(client.UploadString(this.SharpShellURI, ToJson(compilationRequest)));
            }
            catch (WebException e)
            {
                using (var reader = new StreamReader(e.Response.GetResponseStream()))
                {
                    throw new CompilationException("CompilationException: " + reader.ReadToEnd());
                }
            }

        }

        public void SetHeaders()
        {
            this.client.Proxy = WebRequest.DefaultWebProxy;
            this.client.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            this.client.Headers.Set("Content-Type", "application/json");
        }

        private static string ToJson(CompilationRequest request)
        {
            string RequestFormat = 
            @"{{
                ""source"": ""{0}"",
                ""frameworkVersion"": {1},
                ""outputKind"": {2}
              }}";

            return String.Format(RequestFormat, JavaScriptStringEncode(request.Source), request.TargetDotNetVersion.ToString("D"), request.OutputKind.ToString("D"));
        }

        // Adapted from https://github.com/mono/mono/blob/master/mcs/class/System.Web/System.Web/HttpUtility.cs
        public static string JavaScriptStringEncode(string value)
        {
            if (String.IsNullOrEmpty(value)) { return String.Empty; }
            int len = value.Length;
            bool needEncode = false;
            char c;
            for (int i = 0; i < len; i++)
            {
                c = value[i];
                if (c >= 0 && c <= 31 || c == 34 || c == 39 || c == 60 || c == 62 || c == 92)
                {
                    needEncode = true;
                    break;
                }
            }

            if (!needEncode) { return value; }

            var sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                c = value[i];
                if (c >= 0 && c <= 7 || c == 11 || c >= 14 && c <= 31 || c == 39 || c == 60 || c == 62)
                    sb.AppendFormat("\\u{0:x4}", (int)c);
                else switch ((int)c)
                    {
                        case 8:
                            sb.Append("\\b");
                            break;
                        case 9:
                            sb.Append("\\t");
                            break;
                        case 10:
                            sb.Append("\\n");
                            break;
                        case 12:
                            sb.Append("\\f");
                            break;
                        case 13:
                            sb.Append("\\r");
                            break;
                        case 34:
                            sb.Append("\\\"");
                            break;
                        case 92:
                            sb.Append("\\\\");
                            break;
                        default:
                            sb.Append(c);
                            break;
                    }
            }
            return sb.ToString();
        }

        private static byte[] Compress(byte[] bytes)
        {
            byte[] compressedILBytes;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress))
                {
                    deflateStream.Write(bytes, 0, bytes.Length);
                }
                compressedILBytes = memoryStream.ToArray();
            }
            return compressedILBytes;
        }
    }
    
    public class CompilationException : Exception
    {
        public CompilationException()
        {

        }

        public CompilationException(string message) : base(message)
        {

        }

        public CompilationException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
