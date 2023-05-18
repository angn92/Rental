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
            services.AddScoped<IDictionaryDtoHelper, DictionaryDtoHelper>();
            services.AddScoped<ICustomerDtoHelper, CustomerDtoHelper>();
            services.AddScoped<IProductDtoHelper, ProductDtoHelper>();
           

            return services;
        }
    }
}
