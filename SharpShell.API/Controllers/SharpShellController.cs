// Author: Ryan Cobb (@cobbr_io)
// Project: SharpShell (https://github.com/cobbr/SharpShell)
// License: BSD 3-Clause

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using YamlDotNet.Serialization;
using SharpShell.API.Models;

namespace SharpShell.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharpShellController : ControllerBase
    {
        private readonly SharpShellContext _context;

        public SharpShellController(SharpShellContext context)
        {
            this._context = context;
        }

        // POST api/sharpshell/compile
        [HttpPost("compile")]
        public ActionResult<string> PostCompile([FromBody] Compiler.CompilationRequest request = default(Compiler.CompilationRequest))
        {
            request.SourceDirectory = Common.SharpShellSourceDirectory;
            request.ResourceDirectory = Common.SharpShellResourcesDirectory;
            request.ReferenceDirectory = Common.SharpShellReferencesDirectory;
            using (TextReader reader = System.IO.File.OpenText(Common.SharpShellReferencesConfig))
            {
                var deserializer = new DeserializerBuilder().Build();
                request.References = deserializer.Deserialize<List<Compiler.Reference>>(reader)
                    .Where(R => R.Framework == request.TargetDotNetVersion)
                    .Where(R => R.Enabled)
                    .ToList();
            }
            using (TextReader reader = System.IO.File.OpenText(Common.SharpShellResourcesConfig))
            {
                var deserializer = new DeserializerBuilder().Build();
                request.EmbeddedResources = deserializer.Deserialize<List<Compiler.EmbeddedResource>>(reader)
                    .Where(ER => ER.Enabled)
                    .ToList();
            }
            try
            {
                return Convert.ToBase64String(Compiler.Compile(request));
            }
            catch (CompilerException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
