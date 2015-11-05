using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Engine.Utilities;

namespace Artemis.Engine
{
    class ArtemisObject
    {
        /// <summary>
        /// The list of attributes attached to the object
        /// </summary>
        public readonly DynamicAttributeContainer Attributes;

        /// <summary>
        /// Decides whether or not to update object
        /// </summary>
        public bool NeedsUpdate;

        /// <summary>
        /// Sets the ArtemisObject's current updater to 'updater'
        /// </summary>
        public void SetUpdater(Action updater)
        {
            // set updater
        }

        /// <summary>
        /// Updates the ArtemisObject by calling its updater
        /// </summary>
        public void Update()
        {
            // Update
        }
    }
}
