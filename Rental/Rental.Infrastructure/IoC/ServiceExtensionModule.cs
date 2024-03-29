﻿using Autofac;
using Microsoft.AspNetCore.Http;
using Rental.Core.Base;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Services;
using Rental.Infrastructure.Services.EncryptService;
using Rental.Infrastructure.Wrapper;

namespace Rental.Infrastructure.IoC
{
    public class ServiceExtensionModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register ICommandHandler
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(ICommandHandler<,>))
                .AsImplementedInterfaces();

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

            // Register component which use IService
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.IsAssignableTo<IService>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.IsAssignableTo<ICommand>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.IsAssignableTo<IEmailHelper>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<PasswordHelper>()
                .As<IPasswordHelper>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EncryptService>()
                .As<IEncrypt>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationDbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.IsAssignableTo<ProductHelper>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<SessionHelper>()
                .As<ISessionHelper>()
                .InstancePerLifetimeScope();

            builder.RegisterType<HttpContextWrapper>()
                .As<IHttpContextWrapper>()
                .InstancePerLifetimeScope();

            builder.RegisterType<HttpContextAccessor>()
                .As<IHttpContextAccessor>()
                .SingleInstance();

            builder.RegisterType<OrderHelper>()
                .As<IOrderHelper>()
                .InstancePerLifetimeScope();
        }
    }
}
