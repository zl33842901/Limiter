using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using xLiAd.Limiter.Abstractions;

namespace xLiAd.Limiter.Core
{
    public static class Extension
    {
        public static IServiceCollection AddLimiter(IServiceCollection services)
        {
            services.AddSingleton<IKeyProvider, DefaultKeyProvider>();
            services.AddSingleton<ILimitPolicyProvider, DefaultLimitPolicyProvider>();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
