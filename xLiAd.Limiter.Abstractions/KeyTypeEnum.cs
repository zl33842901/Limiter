using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.Limiter.Abstractions
{
    public enum KeyTypeEnum
    {
        None = 0,
        FirstParam = 1,
        SecondParam = 2,
        ThirdParam = 3,

        AllParam = 10,
        ClientIp = 11,
        /// <summary>
        /// 使用参数的属性，如："2:Property1:Filed5"
        /// </summary>
        InParam = 12
    }
}
