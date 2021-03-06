using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Cassandra;
using DSB.Push.Api.Controllers;
using DSB.Push.Api.InternalModels;
using DSB.Push.Repositories;

namespace DSB.Push.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Cassandra DB connector
            var cassandraSettings = _configuration.GetSection("CassandraSettings").Get<CassandraSettingsModel>();
            var clusterBuilder = Cluster.Builder()
                .AddContactPoints(cassandraSettings.ContactPoint)
                .WithPort(cassandraSettings.Port)
                .WithAuthProvider(new PlainTextAuthProvider(cassandraSettings.Username, cassandraSettings.Password))
                .Build();
            services.AddSingleton(x => clusterBuilder.Connect(cassandraSettings.KeySpace));
            
            services.AddScoped<IPushDataRepository, CassandraRepository>();
            
            services.AddControllers();
            
            //Controllers generate lowercase url
            services.AddRouting(options => options.LowercaseUrls = true);
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "DSB Push Notification APIs",
                    Description = "A set of APIs for managing DSB customers' push notifications",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Test"))
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DSB.Push.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}