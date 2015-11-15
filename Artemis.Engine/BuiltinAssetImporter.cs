using Artemis.Engine.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Artemis.Engine
{
    /// <summary>
    /// Builtin IAssetImporter for object types T already known by the ContentManager.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class BuiltinAssetImporter<T> : AbstractAssetImporter
    {
        public override object ImportFrom(string filePath)
        {
            var cfName = AssetLoader.ContentFolderName;
            var fName = DirectoryUtils.MakeRelativePath(
                    cfName, Path.Combine(cfName, filePath));
            var val = AssetLoader.Content.Load<T>("test-image1");
            return val;
        }
    }
}
