using System;
using System.Runtime.Serialization;

namespace MyNote.Utils
{
    [Serializable]
    internal class DatabaseConnectionException : Exception
    {
        private Exception ex;

        public DatabaseConnectionException()
        {
        }

        public DatabaseConnectionException(Exception ex)
        {
            this.ex = ex;
        }

        public DatabaseConnectionException(string message) : base(message)
        {
        }

        public DatabaseConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DatabaseConnectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}