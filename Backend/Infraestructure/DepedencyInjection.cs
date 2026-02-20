using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure
{
    public static class DepedencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services)
        {
            return services;
        }
    }
}
