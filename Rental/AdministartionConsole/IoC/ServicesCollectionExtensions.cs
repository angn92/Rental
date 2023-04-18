using AdministartionConsole.Helpers;
using Rental.Infrastructure.Helpers;

namespace AdministartionConsole.IoC
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection DependencyServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryHelper, CategoryHelper>();
            services.AddScoped<ICategoryDtoHelper, CategoryDtoHelper>();

            return services;
        }
    }
}
