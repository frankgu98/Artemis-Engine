using Artemis.Engine.Utilities;
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
            return AssetLoader.Content.Load<T>(
                DirectoryUtils.MakeRelativePath(
                    cfName, Path.Combine(cfName, filePath)));
        }
    }
}
