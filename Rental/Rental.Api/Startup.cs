using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.IoC;

namespace Rental.Api
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
            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("Request-Header");
                logging.ResponseHeaders.Add("Response-Header");
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            });
            services.AddDbContext<ApplicationDbContext>(ctx => ctx.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<RentalContext>(ctx => ctx.UseInMemoryDatabase(databaseName: "RentalDB"));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rental.Api", Version = "v1" });
            });
        }
        
        //This method register all component.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ModuleApplication());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rental.Api v1"));
            }

            app.UseHttpLogging();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
