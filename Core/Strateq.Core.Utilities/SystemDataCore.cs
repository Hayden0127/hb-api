using System;

namespace Strateq.Core.Utilities
{
    public partial class SystemDataCore
    {
        public partial struct ErrorCode
        {
            public static string Validation = "01"; // validation
            public static string InternalServer = "02";
        }

        public class Logs
        {
            public const string Debug = "Debug";
            public const string Information = "Information";
            public const string Error = "Error";
            public const string Warning = "Warning";
        }

        public partial struct StatusCode
        {
            public static int Success = 200;
            public static int Invalid = 100;
            public static int NotFound = 101;
            public static int InsufficientCredit = 102;
            public static int Exist = 103;
            public static int LoginFail = 104;
        }
    }
}
