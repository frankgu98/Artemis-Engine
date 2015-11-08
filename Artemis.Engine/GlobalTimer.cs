using Microsoft.Xna.Framework;

namespace Artemis.Engine.Utilities
{
    public sealed class GlobalTimer
    {
        /// <summary>
        /// Global reference to the games total time and it's propeties
        /// </summary>
        public GameTime GlobalGameTime { get; private set; }

        /// <summary>
        /// Total time in milliseconds
        /// </summary>
        public double ElapsedTime { get; private set; }

        /// <summary>
        /// Total frames advanced
        /// </summary>
        public int ElapsedFrames { get; private set; }

        /// <summary>
        /// Time change between updates
        /// </summary>
        public double DeltaTime { get; private set; }

        internal GlobalTimer() { }

        /// <summary>
        /// Updates total game time with new time
        /// </summary>
        internal void UpdateTime(GameTime gameTime)
        {
            DeltaTime = gameTime.ElapsedGameTime.TotalMilliseconds;
            GlobalGameTime = gameTime;
            ElapsedTime += DeltaTime;
            ElapsedFrames++;
        }
    }
}
