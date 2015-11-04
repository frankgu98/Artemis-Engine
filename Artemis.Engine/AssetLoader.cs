using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Artemis.Engine
{
    public static class AssetLoader
    {

    	private static ContentManager Content;

    	internal static void Initialize(ContentManager content)
    	{
    		Content = content;
    	}

    	public static Texture2D Texture(string name)
    	{
    		return Content.Load<Texture2D>(name);
    	}

    	public static SpriteFont Font(string name)
    	{
    		return Content.Load<SpriteFont>(name);
    	}

    }
}
