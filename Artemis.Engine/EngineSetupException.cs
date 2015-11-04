using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Artemis.Engine
{
    public class EngineSetupException : Exception
    {
        public EngineSetupException() : base() { }
        public EngineSetupException(string msg) : base(msg) { }
        public EngineSetupException(string msg, Exception inner) : base(msg, inner) { }
        public EngineSetupException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
