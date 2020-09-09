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
        [Limiter(KeyType = Abstractions.KeyTypeEnum.FirstParam, LimitPolicieString = "1h:3")]
        public string DoSomething(int i) => $"I'm DoSomething(int) param:{i}";

        [Limiter(KeyType = Abstractions.KeyTypeEnum.AllParam, LimitPolicieString = "10s:1")]
        public string DoSomething(int i, string s) => $"I'm DoSomething(int, string) param:{i} {s}";

        [Limiter(KeyType = Abstractions.KeyTypeEnum.InParam, InParamKey = "1:P1;2:P3:P2", LimitPolicieString ="5s:1")]
        public string DoSomething(P p1, P p2) => $"I'm DoSomething(P, P)";
    }

    public interface ISampleService
    {
        string DoSomething();
        string DoSomething(int i);
        string DoSomething(int i, string s);
        string DoSomething(P p1, P p2);
    }

    public class P
    {
        public int P1 { get; set; }
        public string P2 { get; set; }

        public P P3 { get; set; }
    }
}
