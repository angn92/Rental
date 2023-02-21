using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Rental.Infrastructure.EF;
using System.Linq;

namespace Rental.Test.IntegrationTest
{
    public class WebApplicationFactoryBase
    {
        public WebApplicationFactory<Program> _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    //Find another config for DB
                    var dbContextConfig = services.SingleOrDefault(context => context.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    //Remove this config and use configuration from test
                    if (dbContextConfig != null)
                        services.Remove(dbContextConfig);

                    services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("MemoryTestDatabase"));
                });
            });
        }
    }
}
