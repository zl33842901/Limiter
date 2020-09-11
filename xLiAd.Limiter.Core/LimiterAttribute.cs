using AspectCore.DynamicProxy;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using xLiAd.Limiter.Abstractions;
using System.Linq;

namespace xLiAd.Limiter.Core
{
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Method, AllowMultiple =true, Inherited =true)]
    public class LimiterAttribute : AbstractInterceptorAttribute
    {
        public KeyTypeEnum KeyType { get; set; }
        public string InParamKey { get; set; }
        public string LimitPolicieString { get; set; }
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            ICacher cacher = context.ServiceProvider.GetService(typeof(ICacher)) as ICacher;
            IKeyProvider keyProvider = context.ServiceProvider.GetService(typeof(IKeyProvider)) as IKeyProvider;
            IHttpContextAccessor httpContextAccessor = context.ServiceProvider.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            var key = keyProvider.ProvideKey(this.KeyType, context.Parameters, httpContextAccessor, this.InParamKey);
            var fullKey = $"xLiAd_Limiter:{context.Implementation.GetType().FullName}_{context.ImplementationMethod.Name}:{key}";
            ILimitPolicyProvider limitPolicyProvider = context.ServiceProvider.GetService(typeof(ILimitPolicyProvider)) as ILimitPolicyProvider;
            var policies = limitPolicyProvider.ParsePolicy(this.LimitPolicieString);
            foreach(var policy in policies)
            {
                var rk = $"{fullKey}:{policy.Key}";
                var list = cacher.Get<List<DateTime>>(rk);
                int n = list?.Count(x => (DateTime.Now - x).TotalSeconds < policy.Key) ?? 0;
                if (n >= policy.Value)
                {
                    throw new OperationLimitException("此操作超过次数限制！");
                }
                else
                {
                    var lr = list ?? new List<DateTime>();
                    lr.Add(DateTime.Now);
                    cacher.Set(rk, lr, TimeSpan.FromSeconds(policy.Key));
                }
            }
            await next(context);
        }
    }
}
