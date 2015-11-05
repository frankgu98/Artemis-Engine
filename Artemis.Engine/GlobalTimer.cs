using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Artemis.Engine.Utilities
{
    class GlobalTimer
    {
        public GameTime GlobalGameTime { get; private set; }
        public double ElapsedTime { get; private set; }
        public int ElapsedFrames { get; private set; }
        public double DeltaTime { get; private set; }

        /// <summary>
        /// Updates elapsed game time
        /// </summary>
        internal void UpdateTime()
        {
            // Update time
        }
    }
}
