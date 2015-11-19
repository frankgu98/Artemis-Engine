using System;
using System.Runtime.Serialization;

namespace Artemis.Engine
{
    /// <summary>
    /// An exception thrown when something goes wrong attempting to register an asset importer.
    /// </summary>
    public class AssetLoadException : Exception
    {
        public AssetLoadException() : base() { }
        public AssetLoadException(string msg) : base(msg) { }
        public AssetLoadException(string msg, Exception inner) : base(msg, inner) { }
        public AssetLoadException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}