using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Engine.Utilities
{
    public class GlobalUpdater
    {
        /// <summary>
        /// List of objects that are to be controlled by the GlobalUpdater
        /// </summary>
        internal List<ArtemisObject> Objects { get; private set; }

        public GlobalUpdater()
        {
            Objects = new List<ArtemisObject>();
        }

        /// <summary>
        /// Adds an object to the global ticker (unless it's already been added)
        /// </summary>
        internal void Add(ArtemisObject obj)
        {
            Objects.Add(obj);
        }

        /// <summary>
        /// Iterate through all internally stored ArtemisObjects and see 
        /// which ones have and haven’t been updated. If they haven’t, call 
        /// their update method. Set their 'NeedsUpdate' flag to true.
        /// </summary>
        internal void FinalizeUpdate()
        {
            foreach (var obj in Objects)
            {
                if (obj.NeedsUpdate)
                {
                    obj.Update();
                }
                obj.NeedsUpdate = true;
            }
        }
    }
}
