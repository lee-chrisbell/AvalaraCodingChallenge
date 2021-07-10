using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvalaraCodingChallenge.TaxCalculator.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax.Repositories;
using AvalaraCodingChallenge.TaxCalculator.Application.Tax.Repositories;
using AvalaraCodingChallenge.TaxCalculator.Application.Tax.Services;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax.Services;

namespace AvalaraCodingChallenge.TaxCalculator.Web
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

            RegisterTaxServices(services);
            RegisterInMemoryContext(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AvalaraCodingChallenge.TaxCalculator.Web", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AvalaraCodingChallenge.TaxCalculator.Web v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void RegisterTaxServices(IServiceCollection services)
        {
            services.AddScoped<ITaxRepository, TaxRepository>();
            services.AddScoped<ITaxService, TaxService>();
        }

        public void RegisterInMemoryContext(IServiceCollection services)
        {
            services.AddDbContext<TaxContext>(opt => {
                opt.UseInMemoryDatabase("TaxDb");
            });

            services.AddScoped<DbContext>(s => s.GetRequiredService<TaxContext>());
        }
    }
}
