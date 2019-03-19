using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalculatorSvc.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.Swagger;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;

using CalculatorSvc.Controllers;

namespace CalculatorSvc
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddControllersAsServices();

            services.AddHealthChecks();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new Info { Title = "Calculator", Version = "v2" });
            });

            services.AddTransient<IstioTracingMiddleware>();

            //Autofac Changes
            services.AddAutofac();

            //Create Container builder
            var builder = new ContainerBuilder();

            //Populate Services from Microsoft DI
            builder.Populate(services);

            //Register DI types or Interfaces
            //add Interceptions by calling Extension metods EnableClassInterceptors or EnableInterfaceInterceptors
            builder.RegisterType<CalculatorController>().EnableClassInterceptors();
            
            //Register Interceptor
            builder.RegisterType<CallLogger>();

            //Build Container
            this.ApplicationContainer = builder.Build();

            services.AddHealthChecks()
                .AddCheck<LivenessHealthCheck>("Liveness", failureStatus: null)
                .AddCheck<ReadinessHealthCheck>("Readiness", failureStatus: null);


            //Return Autofac provider as Serviceprovider
            return new AutofacServiceProvider(this.ApplicationContainer);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHealthChecks("/health/live", new HealthCheckOptions()
            {
                Predicate = check => check.Name == "Liveness"
            });

            app.UseHealthChecks("/health/ready", new HealthCheckOptions()
            {
                Predicate = check => check.Name == "Readiness",

            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Calculator V2");
            });

            app.UseIstioTracingMiddleware();

            app.UseHttpsRedirection();
            app.UseMvc();

        }
    }
}
