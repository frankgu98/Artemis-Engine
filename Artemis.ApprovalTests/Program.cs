#region Using Statements
using Artemis.Engine;
using Microsoft.Xna.Framework.Graphics;
using System;
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
        [STAThread]
        static void Main()
        {
            ArtemisEngine.Setup("gamesetup.xml", Initialize);
        }

        static void Initialize()
        {
            var image = AssetLoader.Load<Texture2D>("text-image1");

            ArtemisEngine.RegisterMultiforms(
                typeof(MainMultiform1),
                typeof(MainMultiform2));
            ArtemisEngine.StartWith("Main2");            
        }
    }

    [NamedMultiform("Main1")]
    public class MainMultiform1 : Multiform
    {
        public override void Construct()
        {
            SetUpdater(() => { Console.WriteLine("Updating 1."); });
            SetRenderer(() => { Console.WriteLine("Rendering 1."); });
        }
    }

    [NamedMultiform("Main2")]
    public class MainMultiform2 : Multiform
    {
        public override void Construct()
        {
            SetUpdater(() => { Console.WriteLine("Updating 2."); });
            SetRenderer(() => { Console.WriteLine("Rendering 2."); });
        }
    }

#endif
}
