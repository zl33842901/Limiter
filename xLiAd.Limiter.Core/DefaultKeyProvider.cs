using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using xLiAd.Limiter.Abstractions;

namespace xLiAd.Limiter.Core
{
    public class DefaultKeyProvider : IKeyProvider
    {
        public string ProvideKey(KeyTypeEnum keyType, object[] parameters, IHttpContextAccessor httpContextAccessor, string inParamKey)
        {
            string result;
            switch (keyType)
            {
                case KeyTypeEnum.FirstParam:
                    result = parameters[0]?.ToString();
                    break;
                case KeyTypeEnum.SecondParam:
                    result = parameters[1]?.ToString();
                    break;
                case KeyTypeEnum.ThirdParam:
                    result = parameters[2]?.ToString();
                    break;
                case KeyTypeEnum.AllParam:
                    result = string.Join("-", parameters.Select(x => x.ToString()));
                    break;
                case KeyTypeEnum.InParam:
                    result = GetInParam(parameters, inParamKey);
                    break;
                case KeyTypeEnum.ClientIp:
                    result = GetIp(httpContextAccessor);
                    break;
                default:
                    result = string.Empty;
                    break;
            }
            return result;
        }

        private string GetInParam(object[] parameters, string inParamKey)
        {
            var pkArray = inParamKey.Split(':');
            var i = Convert.ToInt32(pkArray[0].ToString());
            var obj = parameters[i];
            foreach(var s in pkArray.Skip(1))
            {
                var mem = obj.GetType().GetMember(s, System.Reflection.MemberTypes.Field | System.Reflection.MemberTypes.Property, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).FirstOrDefault();
                if (mem == null)
                    throw new ArgumentException("没有找到属性或字段", s);
                if (mem.MemberType == System.Reflection.MemberTypes.Field)
                    obj = (mem as FieldInfo).GetValue(obj);
                else
                    obj = (mem as PropertyInfo).GetValue(obj);
            }
            return obj.ToString();
        }

        private string GetIp(IHttpContextAccessor httpContextAccessor)
        {
            var context = httpContextAccessor.HttpContext;
            var headers = context.Request.Headers;
            string result = null;
            if (headers.ContainsKey("X-Forwarded-For"))
            {
                string xff = headers["X-Forwarded-For"];
                result = xff.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).FirstOrDefault();
            }
            if (string.IsNullOrEmpty(result))
            {
                if (headers.ContainsKey("X-Real-IP"))
                {
                    string xri = headers["X-Real-IP"];
                    result = xri;
                }
            }
            if (string.IsNullOrEmpty(result))
            {
                result = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
            return result;
        }
    }
}
