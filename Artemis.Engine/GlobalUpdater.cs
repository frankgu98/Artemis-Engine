using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Engine.Utilities
{
    class GlobalUpdater
    {
        /// <summary>
        /// Adds an object to the global ticker (unless it's already been added)
        /// </summary>
        internal void Add(ArtemisObject obj)
        {
            // Add object
        }

        /// <summary>
        /// Iterate through all internally stored ArtemisObjects and see 
        /// which ones have and haven’t been updated. If they have, set their 
        /// 'NeedsUpdate' flag to true. If they haven’t, call their update method
        /// </summary>
        internal void FinalizeUpdate()
        {
            // Finalize
        }
    }
}
