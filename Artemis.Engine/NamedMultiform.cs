using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
