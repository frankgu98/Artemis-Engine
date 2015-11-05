using System;
using System.Runtime.Serialization;

namespace Artemis.Engine
{
    /// <summary>
    /// An exception that occurs when something goes wrong whilst registering a multiform.
    /// </summary>
    public class MultiformRegistrationException : Exception
    {
    	public MultiformRegistrationException() : base() { }
    	public MultiformRegistrationException(string msg) : base(msg) { }
    	public MultiformRegistrationException(string msg, Exception inner) : base(msg, inner) { }
    	public MultiformRegistrationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
