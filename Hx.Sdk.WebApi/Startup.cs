using CSRedis;
using Hx.Sdk.Test.Entity;
using Hx.Sdk.Test.Entity.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
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
            services.AddControllers();
            //ILoggerFactory loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
            
            services.AddDatabaseAccessor(options =>
            {
                options.AddDbPool<DefaultDbContext>();
                options.AddDbPool<IdsDbContext, IdsDbContextLocator>();
            }, db=> 
            { 
                db.MigrationAssemblyName = "Hx.Sdk.Test.Entity";
            });
            services.AddRedisCache(Configuration);
            services.AddCorsAccessor();
            //services.AddDbContext<DefaultDbContext>();
            services.AddCapRabbitMQ(Configuration);
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
            app.UseDatabaseAccessor();
            app.UseCorsAccessor();
            //app.UseCapRabbitMQ();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class Cusstom
    {
        /// <summary>
        /// Ìí¼Óredis·þÎñ
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var redisDB = new CSRedisClient(configuration["RedisSettings"]);
            RedisHelper.Initialization(redisDB);
            services.AddSingleton(redisDB);
            return services;
        }
    }
}
