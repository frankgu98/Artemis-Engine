using System;

namespace Artemis.Engine
{
    public class NamedMultiform : Attribute
    {
        public string Name { get; private set; }
        public NamedMultiform(string name)
        {
            Name = name;
        }
    }
}
