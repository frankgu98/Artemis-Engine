using System;
using System.Runtime.Serialization;

namespace Artemis.Engine
{
    /// <summary>
    /// An exception thrown when something goes wrong in the RenderPipeline.
    /// </summary>
    public class RenderPipelineException : Exception
    {
        public RenderPipelineException() { }
        public RenderPipelineException(string message) : base(message) { }
        public RenderPipelineException(string message, Exception inner) : base(message, inner) { }
        public RenderPipelineException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
