using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Artemis.Engine
{
    public class MultiformRegistrationException : Exception
    {
    	public MultiformRegistrationException() : base() { }
    	public MultiformRegistrationException(string msg) : base(msg) { }
    	public MultiformRegistrationException(string msg, Exception inner) : base(msg, inner) { }
    	public MultiformRegistrationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
