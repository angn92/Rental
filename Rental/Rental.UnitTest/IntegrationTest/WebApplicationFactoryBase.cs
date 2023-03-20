using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Rental.Infrastructure.EF;
using System.Linq;
using System.Net.Http;

namespace Rental.Test.IntegrationTest
{
    public class WebApplicationFactoryBase
    {
        public WebApplicationFactory<Program> _factory;
        public ApplicationDbContext _context;
        public HttpClient _httpClient;

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

                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IHttpContextAccessor, IntegrationTestHttp>();
                });

                
            });

            _context = _factory.Services.GetService<ApplicationDbContext>();

            //_factory.CreateDefaultClient().DefaultRequestHeaders.Add("SessionId", "112234");
        }
    }
}
