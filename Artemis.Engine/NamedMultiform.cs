using System;

namespace Artemis.Engine
{

    [AttributeUsage(AttributeTargets.Class)]
    public class NamedMultiform : Attribute
    {

        public string Name { get; private set; }

        public NamedMultiform(string name)
        {
            Name = name;
        }

    }
}
