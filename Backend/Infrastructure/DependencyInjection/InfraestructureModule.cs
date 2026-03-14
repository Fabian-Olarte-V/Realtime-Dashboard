using Application.Common;
using Autofac;
using Domain.AggregateModels.Tickets;
using Domain.AggregateModels.Users;
using Infrastructure.Auth;
using Infrastructure.Common.SystemClock;
using Infrastructure.Finder.Tickets;
using Infrastructure.Finder.Users;
using Infrastructure.Repository.Tickets;

namespace Infrastructure.DependencyInjection
{
    public class InfraestructureModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserFinder>()
                .As<IUserFinder>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TicketFinder>()
                .As<ITicketFinder>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TicketRepository>()
                .As<ITicketRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<JwtTokenGenerator>()
                .As<IJwtTokenGenerator>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SystemClock>()
                .As<IClock>()
                .SingleInstance();
        }
    }
}
