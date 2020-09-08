using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.Limiter.Abstractions
{
    public interface ICacher
    {
        void Set(string key, object value, TimeSpan timeSpan);

        object Get(string key, Type type);

        T Get<T>(string key);
    }
}
