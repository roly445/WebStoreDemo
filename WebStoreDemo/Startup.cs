using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiantBomb.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using WebStoreDemo.Api;
using WebStoreDemo.Infrastucture;
using WebStoreDemo.Infrastucture.Repositories;
using WebStoreDemo.Infrastucture.Settings;
using WebStoreDemo.Infrastucture.Wrappers;

namespace WebStoreDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddMvc();
            services.Configure<GiantBombSettings>(this.Configuration.GetSection("GiantBombSettings"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IGiantBombRestClient, GiantBombRestClient>();
            services.AddScoped<IGiantBombRestClientWrapper, GiantBombRestClientWrapper>();
            services.AddScoped<ICacheDbRepository, CacheDbRepository>();
            //services.AddScoped<IBasket, Basket>();
            services.AddSession();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
