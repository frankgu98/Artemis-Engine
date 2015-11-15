using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Artemis.Engine
{
    /// <summary>
    /// Because monogame's friggin stupid.
    /// </summary>
    internal class GameKernel : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ArtemisEngine engine;

        public GameKernel(ArtemisEngine engine)
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = engine._GameProperties.ContentFolder;
            this.engine = engine;

            AssetLoader.Initialize(Content);
        }

        sealed protected override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            engine.InitializeRenderPipeline(spriteBatch, GraphicsDevice, graphics);
        }

        sealed protected override void LoadContent() { base.LoadContent(); }

        sealed protected override void UnloadContent() { base.UnloadContent(); }

        sealed protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            engine.Update(gameTime);
        }

        sealed protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            engine.Render();
        }

    }
}
