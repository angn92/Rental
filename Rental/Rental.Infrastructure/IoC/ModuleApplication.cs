using Autofac;
using Rental.Core.Repository;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Services;

namespace Rental.Infrastructure.IoC
{
    public class ModuleApplication : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register ICommandHandler
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .AsImplementedInterfaces();

            // Register CommandDispatcher
            builder.RegisterType<CommandDispatcher>()
                .As<ICommandDispatcher>()
                .InstancePerLifetimeScope();

            // Register IQueryHandler
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(IQueryHandler<,>))
                .AsImplementedInterfaces();

            // Register QueryDispatcher
            builder.RegisterType<QueryDispatcher>()
                .As<IQueryDispatcher>()
                .InstancePerLifetimeScope();

            // Register component which use IRepository
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.IsAssignableTo<IRepository>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Register component which use IService
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.IsAssignableTo<IService>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
