#region Using Statements
using Artemis.Engine;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
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

            AssetLoader.RegisterAssetImporter<string>(new TestAssetImporter(), ".test");

            AssetLoader.PrepareAssetGroup("test-group", SearchOption.AllDirectories);
            var image2 = AssetLoader.Load<Texture2D>("test-group.test-image1");

            var group  = AssetLoader.GetGroup("test-group");
            var image3 = group.GetAsset<Texture2D>("test-image1");

            var text = group.GetAsset<string>("test-asset");
            Console.WriteLine(text);

            ArtemisEngine.RegisterMultiforms(
                new MainMultiform1("Main1"),
                new MainMultiform2("Main2"));
            ArtemisEngine.StartWith("Main2");

            AssetLoader.UnloadAssetGroup("test-group");
        }
    }

    public class MainMultiform1 : Multiform
    {
        public MainMultiform1() : base() { }
        public MainMultiform1(string name) : base(name) { }

        public override void Construct()
        {
            SetUpdater(() => { Console.WriteLine("Updating 1."); });
            SetRenderer(() => { Console.WriteLine("Rendering 1."); });
        }
    }

    public class MainMultiform2 : Multiform
    {
        public MainMultiform2() : base() { }
        public MainMultiform2(string name) : base(name) { }

        public override void Construct()
        {
            SetUpdater(() => { Console.WriteLine("Updating 2."); });
            SetRenderer(() => { Console.WriteLine("Rendering 2."); });
        }
    }

    public class TestAssetImporter : AbstractAssetImporter
    {
        public override object ImportFrom(string filePath)
        {
            string text;
            using (var file = new StreamReader(filePath))
            {
                text = file.ReadToEnd();
            }
            return text;
        }
    }

#endif
}
