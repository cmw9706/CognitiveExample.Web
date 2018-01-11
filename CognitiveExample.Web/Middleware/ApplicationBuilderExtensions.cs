using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseNodeModules(
            this IApplicationBuilder app, string root)
        {
            var path = Path.Combine(root, "node_modules");
            var options = new StaticFileOptions
            {
                RequestPath = "/node_modules",
                FileProvider = new PhysicalFileProvider(path)
            };

            app.UseStaticFiles(options);

            return app;
        }
    }
}
