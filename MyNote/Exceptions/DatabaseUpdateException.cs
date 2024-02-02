using System;
using System.Runtime.Serialization;

namespace MyNote
{
    [Serializable]
    internal class DatabaseUpdateException : Exception
    {
        public DatabaseUpdateException()
        {
        }

        public DatabaseUpdateException(string message) : base(message)
        {
        }

        public DatabaseUpdateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DatabaseUpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}