using Hx.Sdk.Test.Entity;
using Hx.Sdk.Test.Entity.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Hx.Sdk.WebApi
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
            var assembly =Assembly.Load("Hx.Sdk.Core");
            var appType = assembly.GetType("Hx.Sdk.Core.App");
            var member = appType.GetField("EffectiveTypes", BindingFlags.Public | BindingFlags.Static);
            var value = (IEnumerable<Type>)member.GetValue(appType);
            services.AddControllers();
            services.AddDatabaseAccessor(options =>
            {
                options.AddDbPool<DefaultDbContext>();
                options.AddDbPool<IdsDbContext, IdsDbContextLocator>();
            }, "Hx.Sdk.Test.Entity");
            services.AddRedisCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
