using Application.Common;
using Application.Common.Behaviors;
using Autofac;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<MediatRAssemblyReference>();
            });

            services.AddValidatorsFromAssemblyContaining<MediatRAssemblyReference>();         
            return services;
        }
    }

    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(ValidationBehavior<,>))
               .As(typeof(IPipelineBehavior<,>))
               .InstancePerDependency();
        }
    }
}
