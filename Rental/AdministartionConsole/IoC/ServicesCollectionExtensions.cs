using Rental.Infrastructure.Helpers;

namespace AdministartionConsole.IoC
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection DependencyServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryHelper, CategoryHelper>();

            return services;
        }
    }
}
