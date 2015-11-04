using System.Collections.Generic;

namespace Artemis.Engine
{
    internal abstract class MultiformPostUpdateEvent
    {

        public abstract void Perform(
            Dictionary<string, Multiform> registered,
            Dictionary<string, Multiform> active);

    }
}
