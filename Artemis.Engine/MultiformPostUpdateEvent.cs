using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Engine
{
    internal abstract class MultiformPostUpdateEvent
    {

        public abstract void Perform(
            Dictionary<string, Multiform> registered,
            Dictionary<string, Multiform> active);

    }
}
