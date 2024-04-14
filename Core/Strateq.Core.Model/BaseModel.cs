using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strateq.Core.Model
{
    public class RequestModelBase
    {
        public string Timestamp { get; set; } = GetTimestamp(DateTime.UtcNow);

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }

    public class ResponseModelBase
    {
        public string Timestamp { get; set; } = GetTimestamp(DateTime.UtcNow);
        public bool Success { get; set; }
        public int StatusCode { get; set; }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
