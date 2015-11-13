using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Artemis.Engine
{

    /// <summary>
    /// The global AssetLoader.
    /// </summary>
    public static class AssetLoader
    {

        /// <summary>
        /// The separator token for an Asset URI. An Asset URI is a string
        /// that indicates the location in an AssetGroup that an asset is
        /// found at. For example, "Shared.Images.WallTexture1" is the asset
        /// with name "WallTexture1" in the subgroup "Shared.Images" of the
        /// group "Shared".
        /// </summary>
        internal const char ASSET_URI_SEPARATOR = '.';

        /// <summary>
        /// The global ContentManager.
        /// </summary>
    	internal static ContentManager Content;

        /// <summary>
        /// The internal dictionary of all asset groups currently loaded in memory.
        /// </summary>
        private static Dictionary<string, AssetGroup> LoadedAssetGroups = new Dictionary<string, AssetGroup>();

        /// <summary>
        /// The dictionary of asset importers associated with certain types.
        /// </summary>
        private static Dictionary<Type, AbstractAssetImporter> RegisteredAssetImportersByType
            = new Dictionary<Type, AbstractAssetImporter>();

        /// <summary>
        /// The dictionary of asset importers associated with certain extensions.
        /// </summary>
        private static Dictionary<string, AbstractAssetImporter> RegisteredAssetImportersByExtension
            = new Dictionary<string, AbstractAssetImporter>();

        /// <summary>
        /// Initialize the AssetLoader by supplying the ContentManager.
        /// </summary>
        /// <param name="content"></param>
    	internal static void Initialize(ContentManager content)
    	{
    		Content = content;

            // Add default content loaders to the game.

            var texture2DImporter  = new Texture2DAssetImporter();
            var spritefontImporter = new SpriteFontAssetImporter();

            /* For Android
            RegisterAssetImporter<Texture2D>(
                texture2DImporter, "jpg", "bmp", "jpeg", "png", "gif");
            */

            RegisterAssetImporter<Texture2D>(
                texture2DImporter, "jpg", "bmp", "jpeg", "png", "gif", "pict", "tga");

            RegisterAssetImporter<SpriteFont>(
                spritefontImporter, "spritefont");
        }

        /// <summary>
        /// The full name of the content folder.
        /// </summary>
        public static string ContentFolderName
        {
            get
            {
                // Check this to make sure it's not stupid. (i.e. make sure it doesn't cause
                // any strange errors in different contexts)

                return Path.Combine(
                    Directory.GetCurrentDirectory(),
                    Content.RootDirectory);
            }
        }

        /// <summary>
        /// Register an AssetImporter object to be used when attempting to load an asset
        /// of the given type, or an asset file with one of the given extensions.
        /// </summary>
        /// <param name="assetImporter"></param>
        /// <param name="assetFileExtensions"></param>
        public static void RegisterAssetImporter<T>(
            AbstractAssetImporter assetImporter, params string[] assetFileExtensions)
        {
            var assetType = typeof(T);
            if (RegisteredAssetImportersByType.ContainsKey(assetType))
            {
                throw new AssetImporterRegistrationException(
                    String.Format("An asset importer has already been " +
                                  "associated with the type '{0}'.", assetType));
            }
            RegisteredAssetImportersByType.Add(assetType, assetImporter);
            RegisterAssetImporter(assetImporter, assetFileExtensions);
        }

        /// <summary>
        /// Register an AssetImporter object to b used when attempting to load an asset
        /// file with one of the given extensions.
        /// 
        /// This is useful if you have multiple different asset importers for the same
        /// asset type but different asset file extensions. If two asset importers have
        /// the same associated asset type, but different asset file extensions, the
        /// AssetLoader will be able to determine the asset importer to use when given a
        /// file, but will not be able to distinguish the asset importer to use when
        /// given the asset type.
        /// 
        /// To further illustrate:
        /// <code>
        ///     AssetLoader.RegisterAssetImporter<Texture2D>(new AssetImporter1(), "doc", "xml");
        ///     AssetLoader.RegisterAssetImporter(new AssetImporter2(), "docx");
        /// </code>
        /// 
        /// In the above example, it is assumed that AssetImporter1 and AssetImporter2 both import
        /// Texture2D objects. Calling AssetLoader.Load<Texture2D>(fileName) would use AssetImporter1
        /// to import the asset, as well as AssetLoader.LoadUsingExtension("something.doc"), but
        /// AssetLoader.LoadUsingExtension("something.docx") would use AssetImporter2, and
        /// AssetLoader.Load<Texture2D>("something.docx") would *still* attempt to use AssetImporter1,
        /// since that's the only asset importer associated with Texture2D.
        /// </summary>
        /// <param name="assetImporter"></param>
        /// <param name="assetFileExtensions"></param>
        public static void RegisterAssetImporter(
            AbstractAssetImporter assetImporter, params string[] assetFileExtensions)
        {
            foreach (var extension in assetFileExtensions)
            {
                if (RegisteredAssetImportersByExtension.ContainsKey(extension))
                {
                    throw new AssetImporterRegistrationException(
                        String.Format("An asset importer has already been " + 
                                      "associated with the extension '.{0}'", extension));
                }
                RegisteredAssetImportersByExtension.Add(extension, assetImporter);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="treatAsAssetURI"></param>
        /// <returns></returns>
        public static T Load<T>(string name, bool treatAsAssetURI = true)
        {
            if (name.Contains(ASSET_URI_SEPARATOR) && treatAsAssetURI)
            {
                return AssetFromURI<T>(name);
            }
            return (T)RegisteredAssetImportersByType[typeof(T)].ImportFrom(name);
        }

        /// <summary>
        /// Get an asset from an Asset URI.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        private static T AssetFromURI<T>(string name)
        {
            var firstSeparatorPos = name.IndexOf('.');
            var rootGroupName = name.Substring(0, firstSeparatorPos);
            var remainingName = name.Substring(firstSeparatorPos + 1);
            return LoadedAssetGroups[rootGroupName].GetAsset<T>(remainingName);
        }

        /// <summary>
        /// Try to determine the type of file to be loaded based on the extension
        /// type of the file and loads it as said asset.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object LoadAssetUsingExtension(string name)
        {
            var extension = Path.GetExtension(name);
            if (string.IsNullOrEmpty(extension))
            {
                throw new ArgumentException(
                    String.Format("Cannot load asset '{0}'.", name)
                    );
            }
            
            if (!RegisteredAssetImportersByExtension.ContainsKey(name))
            {
                throw new AssetImportException(
                    String.Format("No asset importer for asset with extension '.{0}'.", name)
                    );
            }
            var importer = RegisteredAssetImportersByExtension[name];
            return importer.ImportFrom(name);
        }

        /// <summary>
        /// Load an entire asset group.
        /// </summary>
        public static void PrepareAssetGroup( string name
                                            , SearchOption searchOption
                                            , string fileSearchQuery    = "*"
                                            , string folderSearchQuery  = "*"
                                            , bool pruneEmptyGroups     = true )
        {
            LoadedAssetGroups.Add(
                name, new AssetGroup(
                    Path.Combine(ContentFolderName, name), searchOption, 
                    fileSearchQuery, folderSearchQuery, pruneEmptyGroups)
                    );
        }

        /// <summary>
        /// Return a group with the given full name.
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static AssetGroup GetGroup(string fullName)
        {
            var parts = fullName.Split(ASSET_URI_SEPARATOR);

            if (parts.Length == 1)
            {
                return LoadedAssetGroups[parts[0]];
            }
            else
            {
                return LoadedAssetGroups[parts[0]].GetSubgroup(parts.Skip(1).ToArray());
            }
        }

        /// <summary>
        /// Unload the asset group with the given name.
        /// </summary>
        /// <param name="name"></param>
        public static void UnloadAssetGroup(string name, bool forceGC = true)
        {
            if (name.Contains(ASSET_URI_SEPARATOR))
            {
                var index = name.IndexOf(ASSET_URI_SEPARATOR);
                LoadedAssetGroups[name.Substring(0, index)]
                    .RemoveSubgroup(name.Substring(index + 1));                
            }
            else
            {
                LoadedAssetGroups[name].Dispose();

                LoadedAssetGroups.Remove(name);
            }

            if (forceGC)
            {
                // Force garbage collection. Unloading an AssetGroup is disposing of
                // (usually) a very large number of objects.
                GC.Collect();
            }
        }
    }
}
