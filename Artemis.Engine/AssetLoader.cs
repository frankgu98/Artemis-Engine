using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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

    }
}
