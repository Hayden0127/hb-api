using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strateq.Core.API.Exceptions
{
    public class CustomValidationException : Exception
    {
        public CustomValidationException(string key, string errorMessage) : base(errorMessage) => Key = key ?? throw new ArgumentNullException(nameof(key));

        public string Key { get; set; }
    }
}
