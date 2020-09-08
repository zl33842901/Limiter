using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.Limiter.Abstractions
{
    public interface ILimitPolicyProvider
    {
        LimitPolicy ParsePolicy(string str);
    }
}
