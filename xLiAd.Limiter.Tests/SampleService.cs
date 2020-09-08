using System;
using System.Collections.Generic;
using System.Text;
using xLiAd.Limiter.Core;

namespace xLiAd.Limiter.Tests
{
    public class SampleService : ISampleService
    {
        [Limiter(KeyType = Abstractions.KeyTypeEnum.None, LimitPolicieString = "1minutes:2")]
        public string DoSomething() => $"I'm {nameof(SampleService)}";
    }

    public interface ISampleService
    {
        string DoSomething();
    }
}
