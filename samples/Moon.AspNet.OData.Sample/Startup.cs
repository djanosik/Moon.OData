using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Framework.DependencyInjection;
using Moon.AspNet.Mvc;

namespace Moon.AspNet.OData.Sample
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddOData();
            services.AddSingleton<IRazorViewEngine, ServerViewEngine>();
        }
    }
}