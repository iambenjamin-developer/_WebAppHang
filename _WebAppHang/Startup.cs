using _WebAppHang.Extensions;
using _WebAppHang.Services;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _WebAppHang
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
            services.ConfigureHangFire(Configuration);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "_WebAppHang", Version = "v1" });
            });

            services.AddScoped<IGuidService, GuidService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
                                IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "_WebAppHang v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHangfireDashboard();
            });

            app.UseHangfireDashboard();

            /*
            backgroundJobClient.Enqueue(() => Console.WriteLine("Hola desde Hangfire"));

            backgroundJobClient.Schedule(() => Console.WriteLine("Tarea programada"), TimeSpan.FromSeconds(30));

            recurringJobManager.AddOrUpdate("Esto correra cada 1 minuto",
                                            () => Console.WriteLine("Esto es una tarea recurrente"),
                                            Cron.Minutely);

            */

            var myService = serviceProvider.GetRequiredService<IGuidService>();

            recurringJobManager.AddOrUpdate("Esto correra cada 1 minuto",
                                () => myService.GetRandomIdentifier(),
                                Cron.Minutely);
        }
    }
}
