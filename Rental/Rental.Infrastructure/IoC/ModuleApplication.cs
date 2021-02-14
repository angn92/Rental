using Autofac;
using Rental.Core.Repository;
using Rental.Infrastructure.Commands;
using Rental.Infrastructure.Repositories;
using Rental.Infrastructure.Services.UserService;

namespace Rental.Infrastructure.IoC
{
    public class ModuleApplication : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(ModuleApplication)
                .GetType()
                .Assembly;

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .AsImplementedInterfaces();

            builder.RegisterType<CommandDispatcher>()
                .As<ICommandDispatcher>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserService>()
                .As<IUserService>()
                .AsImplementedInterfaces();
        }
    }
}
