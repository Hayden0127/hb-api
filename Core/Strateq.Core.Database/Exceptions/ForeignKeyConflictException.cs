using System;
using System.Runtime.Serialization;

namespace Strateq.Core.API.Exceptions
{
    [Serializable]
    public class ForeignKeyConflictException : Exception
    {
        public ForeignKeyConflictException()
        {
        }

        public ForeignKeyConflictException(string message) : base(message)
        {
        }

        public ForeignKeyConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ForeignKeyConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}