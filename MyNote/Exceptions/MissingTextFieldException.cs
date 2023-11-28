using System;
using System.Runtime.Serialization;

namespace MyNote
{
    [Serializable]
    internal class MissingTextFieldException : Exception
    {
        public MissingTextFieldException()
        {
        }

        public MissingTextFieldException(string message) : base(message)
        {
        }

        public MissingTextFieldException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MissingTextFieldException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}