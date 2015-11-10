using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;

namespace Artemis.Engine
{

    /// <summary>
    /// The global AssetLoader.
    /// </summary>
    public static class AssetLoader
    {

        /// <summary>
        /// The global ContentManager.
        /// </summary>
    	private static ContentManager Content;

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
    	public static Texture2D Texture(string name)
    	{
    		return Content.Load<Texture2D>(name);
    	}

        /// <summary>
        /// Load a SpriteFont with the given path.
        /// </summary>
    	public static SpriteFont Font(string name)
    	{
    		return Content.Load<SpriteFont>(name);
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
                        String.Format("Do not know how to load asset with extension '{0}'.", extension)
                        );
            }
        }
    }
}
