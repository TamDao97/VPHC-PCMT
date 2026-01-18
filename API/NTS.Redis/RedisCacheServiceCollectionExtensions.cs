using Microsoft.Extensions.DependencyInjection;

namespace NTS.Redis
{
    public static class RedisCacheServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services)
        {
            services.AddSingleton<IRedisCacheConnectionFactory, RedisCacheConnectionFactory>();
            services.AddSingleton<RedisCacheService>();

            return services;
        }
    }
}
