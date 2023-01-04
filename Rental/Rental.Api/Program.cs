using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rental.Infrastructure.Configuration;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.IoC;
using RentalCommon.Headers;
using System;
using System.IO;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging((x) => x.SetMinimumLevel(LogLevel.Information).AddConsole());

//Default middleware logging in .NET 6
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestBody | HttpLoggingFields.RequestPropertiesAndHeaders |
                            HttpLoggingFields.ResponseBody | HttpLoggingFields.ResponsePropertiesAndHeaders;
});

//Autofac 
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(x => x.RegisterModule(new ServiceExtensionModule()));

builder.Services.AddControllers();

//Get connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Set correct database  
var memoryDb = builder.Configuration.GetSection("InMemoryDatabase");

if (memoryDb.Value.Equals("True"))
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("testDB"));
else
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));


builder.Services.AddEndpointsApiExplorer();

//Configuration for comments in swagger GUI
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.Configure<ConfigurationOptions>(builder.Configuration.GetSection("ConfigurationOptions"));

var app = builder.Build();

//Enable HTTP logging 
app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rental.Api v1"));
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMiddleware<ResponseMiddleware>();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());


app.Run();