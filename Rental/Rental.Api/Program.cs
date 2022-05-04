using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rental.Infrastructure.Configuration;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.IoC;
using RentalCommon.Headers;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSwaggerGen();
builder.Configuration.GetSection("Registration").Get<ConfigurationOptions>();

var app = builder.Build();

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