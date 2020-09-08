using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.Limiter.Abstractions
{
    public interface IKeyProvider
    {
        string ProvideKey(KeyTypeEnum keyType, object[] parameters, IHttpContextAccessor httpContextAccessor, string inParamKey);
    }
}
