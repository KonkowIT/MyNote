using System;
using System.Runtime.Serialization;

namespace MyNote
{
    [Serializable]
    internal class InvalidPswdFieldException : Exception
    {
        public InvalidPswdFieldException()
        {
        }

        public InvalidPswdFieldException(string message) : base(message)
        {
        }

        public InvalidPswdFieldException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidPswdFieldException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}