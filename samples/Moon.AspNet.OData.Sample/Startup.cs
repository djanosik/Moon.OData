using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Moon.AspNet.Mvc;

namespace Moon.AspNet.OData.Sample
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app
                .UseIISPlatformHandler()
                .UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<IRazorViewEngine, ServerViewEngine>()
                .AddMvc()
                .AddOData();
        }
    }
}