using System;
using System.Runtime.Serialization;

namespace Artemis.Engine
{
    /// <summary>
    /// An exception that occurs when something goes wrong in a Multiform.
    /// </summary>
    public class MultiformException : Exception
    {
        public MultiformException() : base() { }
        public MultiformException(string msg) : base(msg) { }
        public MultiformException(string msg, Exception inner) : base(msg, inner) { }
        public MultiformException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
