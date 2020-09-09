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
        private ISampleService Before()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<ISampleService, SampleService>();
            services.AddLimiterMemory();
            var container = services.ToServiceContainer();
            var sr = container.Build();

            var serv = sr.GetService<ISampleService>();
            return serv;
        }

        [Fact]
        public void TestService1()
        {
            var serv = Before();

            serv.DoSomething();
            serv.DoSomething();
            Assert.Throws<AspectCore.DynamicProxy.AspectInvocationException>(serv.DoSomething);
        }

        [Fact]
        public void TestService2()
        {
            var serv = Before();

            serv.DoSomething(2);
            serv.DoSomething(2);
            serv.DoSomething(2);

            serv.DoSomething(3);
            Assert.Throws<AspectCore.DynamicProxy.AspectInvocationException>(() => serv.DoSomething(2));
            serv.DoSomething(3);
        }

        [Fact]
        public void TestService3()
        {
            var serv = Before();

            serv.DoSomething(2, "a");
            serv.DoSomething(2, null);
            serv.DoSomething(2, string.Empty);
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

        [Fact]
        public void TestKeyProvider()
        {
            var keyProvider = new DefaultKeyProvider();
            var p1 = new P() { P1 = 1, P2 = "abc" };
            var p2 = new P() { P3 = p1 };
            var key = keyProvider.ProvideKey(Abstractions.KeyTypeEnum.InParam, new object[] { p1, p2 }, null, "1:P1;2:P3:P2");
            Assert.Equal("1-abc", key);
        }
    }
}
