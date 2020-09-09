using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.Limiter.Core
{
    public class OperationLimitException : InvalidOperationException
    {
        public OperationLimitException() { }
        public OperationLimitException(string message) : base(message) { }
        public OperationLimitException(string message, Exception innerException) : base(message, innerException) { }
    }
}
