using Microsoft.Extensions.DependencyInjection;
using System;
using xLiAd.Limiter.Abstractions;
using xLiAd.Limiter.Core;

namespace xLiAd.Limiter.Memory
{
    public static class Extension
    {
        public static IServiceCollection AddLimiterMemory(this IServiceCollection services)
        {
            services.AddSingleton<ICacher, MemoryCacher>();
            services.AddSingleton<IKeyProvider, DefaultKeyProvider>();
            services.AddSingleton<ILimitPolicyProvider, DefaultLimitPolicyProvider>();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
