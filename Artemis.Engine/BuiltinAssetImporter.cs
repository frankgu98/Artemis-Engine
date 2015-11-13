using Artemis.Engine.Utilities;
using Microsoft.Xna.Framework.Graphics;

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
            return AssetLoader.Content.Load<T>(
                DirectoryUtils.MakeRelativePath(AssetLoader.ContentFolderName, filePath));
        }
    }
}
