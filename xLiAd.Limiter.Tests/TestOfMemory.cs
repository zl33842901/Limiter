using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Microsoft.Extensions.DependencyInjection;
using System;
using xLiAd.Limiter.Core;
using xLiAd.Limiter.Memory;
using Xunit;

namespace xLiAd.Limiter.Tests
{
    public class TestOfMemory
    {
        [Fact]
        public void TestService()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<ISampleService, SampleService>();
            services.AddLimiterMemory();
            var container = services.ToServiceContainer();
            var sr = container.Build();

            var serv = sr.GetService<ISampleService>();

            serv.DoSomething();
            serv.DoSomething();
            serv.DoSomething();
        }


        [Fact]
        public void TestLimitPolicy1()
        {
            var limitPolicy = new DefaultLimitPolicyProvider();
            var lp = limitPolicy.ParsePolicy("2seconds:50;1minutes:100");
            Assert.Equal(2, lp.Count);
            Assert.Equal(50, lp[2]);
            Assert.Equal(100, lp[60]);
        }
    }
}
