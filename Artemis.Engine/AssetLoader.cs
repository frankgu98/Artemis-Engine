using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    	private static ContentManager Content;

        /// <summary>
        /// The internal dictionary of all asset groups currently loaded in memory.
        /// </summary>
        private static Dictionary<string, AssetGroup> LoadedAssetGroups = new Dictionary<string, AssetGroup>();

        /// <summary>
        /// Initialize the AssetLoader by supplying the ContentManager.
        /// </summary>
        /// <param name="content"></param>
    	internal static void Initialize(ContentManager content)
    	{
    		Content = content;
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
        /// Load a Texture2D with the given path.
        /// </summary>
    	public static Texture2D Texture(string name, bool treatAsAssetURI = true)
    	{
            if (name.Contains(ASSET_URI_SEPARATOR) && treatAsAssetURI)
            {
                return AssetFromURI<Texture2D>(name);
            }
    		return Content.Load<Texture2D>(name);
    	}

        /// <summary>
        /// Load a SpriteFont with the given path.
        /// </summary>
    	public static SpriteFont Font(string name, bool treatAsAssetURI = true)
    	{
            if (name.Contains(ASSET_URI_SEPARATOR) && treatAsAssetURI)
            {
                return AssetFromURI<SpriteFont>(name);
            }
    		return Content.Load<SpriteFont>(name);
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
        /// Tries to determine the type of file to be loaded based on the extension
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
            var nameWithoutExtension = Path.GetFileNameWithoutExtension(name);

            switch (extension)
            {
                case ".png":
                case ".jpg":
                case ".jpeg":
                    return Texture(nameWithoutExtension);
                case ".spritefont":
                    return Font(nameWithoutExtension);
                default:
                    throw new ArgumentException(
                        String.Format("Unknown asset extension '{0}'.", extension)
                        );
            }
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
        public static void UnloadAssetGroup(string name)
        {
            // LoadedAssetGroups[name].Dispose(true);

            LoadedAssetGroups.Remove(name);
        }
    }
}
