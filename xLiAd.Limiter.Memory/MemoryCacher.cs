using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using xLiAd.Limiter.Abstractions;

namespace xLiAd.Limiter.Memory
{
    public class MemoryCacher : ICacher
    {
        private static readonly MemoryCache mc = new MemoryCache(new MemoryCacheOptions() { });

        public void Set(string key, object value, TimeSpan timeSpan)
        {
            mc.Set(key, value, timeSpan);
        }

        public object Get(string key)
        {
            return mc.Get(key);
        }
        public T Get<T>(string key)
        {
            var o = (T)Get(key);
            return o;
        }
        public object Get(string key, Type type)
        {
            return Get(key);
        }
    }
}
