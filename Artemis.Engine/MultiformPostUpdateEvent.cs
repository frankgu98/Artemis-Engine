using System.Collections.Generic;

namespace Artemis.Engine
{
    /// <summary>
    /// An object which represents an event to perform on the multiform manager after
    /// the update loop has finished.
    /// </summary>
    internal abstract class MultiformPostUpdateEvent
    {

        public abstract void Perform(
            Dictionary<string, Multiform> registered,
            Dictionary<string, Multiform> active);

    }
}
