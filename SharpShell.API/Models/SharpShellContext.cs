// Author: Ryan Cobb (@cobbr_io)
// Project: SharpShell (https://github.com/cobbr/SharpShell)
// License: BSD 3-Clause

using Microsoft.EntityFrameworkCore;

namespace SharpShell.API.Models
{
    public class SharpShellContext : DbContext
    {
        public SharpShellContext(DbContextOptions<SharpShellContext> options) : base(options)
        {

        }
    }
}
