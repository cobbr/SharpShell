// Author: Ryan Cobb (@cobbr_io)
// Project: SharpShell (https://github.com/cobbr/SharpShell)
// License: BSD 3-Clause

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SharpShell.API.Models;

namespace SharpShell.API
{
    public class SharpShellAPI
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<SharpShellContext>();
                var configuration = services.GetRequiredService<IConfiguration>();
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            new WebHostBuilder()
                .UseKestrel(options =>
                {
                    options.ListenAnyIP(5000);
                })
                .UseStartup<Startup>();
    }
}
