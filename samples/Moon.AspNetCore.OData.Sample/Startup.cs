using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace Moon.AspNetCore.OData.Sample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .Configure<RazorViewEngineOptions>(o =>
                {
                    o.ViewLocationFormats.Clear();
                    o.ViewLocationFormats.Add("/Server/{1}/{0}.cshtml");
                    o.ViewLocationFormats.Add("/Server/Shared/{0}.cshtml");
                })
                .AddMvc()
                .AddOData();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}