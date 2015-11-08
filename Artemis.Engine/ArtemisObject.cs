using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Engine.Utilities;

namespace Artemis.Engine
{
    public class ArtemisObject
    {
        /// <summary>
        /// The list of attributes attached to the object
        /// </summary>
        public readonly DynamicAttributeContainer Attributes;

        /// <summary>
        /// Decides whether or not to update object
        /// </summary>
        public bool NeedsUpdate { get; set; }

        private Action updater;

        /// <summary>
        /// Sets the ArtemisObject's current updater to 'updater'
        /// </summary>
        public void SetUpdater(Action updater)
        {
            this.updater = updater;
        }

        /// <summary>
        /// Updates the ArtemisObject by calling its updater
        /// </summary>
        public void Update()
        {
            if (updater != null)
            {
                updater();
            }

            NeedsUpdate = false;
        }
    }
}