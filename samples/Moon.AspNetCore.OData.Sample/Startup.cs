using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Moon.AspNetCore.OData.Sample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
            => services
                .AddMvc()
                .AddOData();

        public void Configure(IApplicationBuilder app)
            => app.UseMvc();
    }
}