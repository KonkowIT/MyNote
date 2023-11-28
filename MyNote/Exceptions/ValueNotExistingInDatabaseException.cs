using System;
using System.Runtime.Serialization;

namespace MyNote
{
    [Serializable]
    internal class ValueNotExistingInDatabaseException : Exception
    {
        public ValueNotExistingInDatabaseException()
        {
        }

        public ValueNotExistingInDatabaseException(string message) : base(message)
        {
        }

        public ValueNotExistingInDatabaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ValueNotExistingInDatabaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}