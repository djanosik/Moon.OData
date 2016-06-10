using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Moon.AspNetCore.Mvc;

namespace Moon.AspNetCore.OData.Sample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<IRazorViewEngine, ServerViewEngine>()
                .AddMvc()
                .AddOData();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}