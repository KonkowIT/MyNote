using System;
using System.Runtime.Serialization;

namespace MyNote
{
    [Serializable]
    internal class InvalidEmailFieldException : Exception
    {
        public InvalidEmailFieldException()
        {
        }

        public InvalidEmailFieldException(string message) : base(message)
        {
        }

        public InvalidEmailFieldException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidEmailFieldException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}