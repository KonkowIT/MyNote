using System;
using System.Runtime.Serialization;

namespace MyNote.Utils
{
    [Serializable]
    internal class DatabaseInsertException : Exception
    {
        public DatabaseInsertException()
        {
        }

        public DatabaseInsertException(string message) : base(message)
        {
        }

        public DatabaseInsertException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DatabaseInsertException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}