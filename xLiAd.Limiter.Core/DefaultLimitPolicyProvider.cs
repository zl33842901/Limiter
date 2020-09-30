using System;
using System.Collections.Generic;
using System.Text;
using xLiAd.Limiter.Abstractions;
using System.Text.RegularExpressions;
using System.Linq;

namespace xLiAd.Limiter.Core
{
    public class DefaultLimitPolicyProvider : ILimitPolicyProvider
    {
        private string[] SecondSuffix = new string[] { "seconds", "s", "秒" };
        private string[] MinuteSuffix = new string[] { "minutes", "m", "分钟", "分" };
        private string[] HourSuffix = new string[] { "hours", "h", "小时" };
        private Dictionary<string, int> SuffixBy => SecondSuffix.ToDictionary(x => x, x => 1).Union(
            MinuteSuffix.ToDictionary(x => x, x => 60))
            .Union(HourSuffix.ToDictionary(x => x, x => 3600)).ToDictionary(x => x.Key, x => x.Value);
        public LimitPolicy ParsePolicy(string str)
        {
            var strArray = str.Split(',', ';').Where(x => !string.IsNullOrEmpty(x));
            LimitPolicy result = new LimitPolicy();
            foreach(var s in strArray)
            {
                var rule = s.Split(':');
                if (rule.Length < 2)
                    throw new Exception($"unknown policy string : {s}");
                var seconds = ConvertToSecond(rule[0]);
                var times = Convert.ToInt32(rule[1]);
                result.Add(seconds, times);
            }
            return result;
        }

        private int ConvertToSecond(string s)
        {
            foreach(var suffix in SuffixBy)
            {
                var match = Regex.Match(s, $"([\\d]+){suffix.Key}", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return GetGroupLast(match.Groups) * suffix.Value;
                }
            }
            throw new Exception($"unknown policy string : {s}");
        }

        private int GetGroupLast(GroupCollection groupCollection)
        {
            string result = null;
            foreach(Group group in groupCollection)
            {
                result = group.Value;
            }
            return Convert.ToInt32(result);
        }
    }
}
