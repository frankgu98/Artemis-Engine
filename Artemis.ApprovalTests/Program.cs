#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Artemis.Engine;
using Artemis.Engine.Input;
using Artemis.Engine.Utilities;
using Microsoft.Xna.Framework;
#endregion

namespace Artemis.ApprovalTests
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ArtemisEngine.Setup(
                new Resolution(800, 600), false, false, true, 
                false, true, false, true, Color.Black, "Test1");

            ArtemisEngine.Begin();
        }
    }
#endif
}
