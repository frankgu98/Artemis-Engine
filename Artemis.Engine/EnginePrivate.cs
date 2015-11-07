using Artemis.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Artemis.Engine
{
    public sealed partial class Engine : Game
    {
        
        /// <summary>
        /// The main game graphics device manager.
        /// </summary>
        GraphicsDeviceManager graphics;

        /// <summary>
        /// The game spriteBatch. The user doesn't interact with this, instead they
        /// render by interfacing with RenderPipeline.
        /// </summary>
        SpriteBatch spriteBatch;

        // Private instance fields

        private RenderPipeline _RenderPipeline;
        private MultiformManager _MultiformManager;
        private GameProperties _GameProperties;

        private MouseInput _Mouse;
        private KeyboardInput _Keyboard;

        private Engine(GameProperties properties) 
        {
            _GameProperties = properties;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = properties.ContentFolder;

            Initialize();
        }

        /// <summary>
        /// Required override of Monogame.Game.Initialize. Not actually part of the Artemis Engine.
        /// </summary>
        sealed protected override void Initialize()
        {
            base.Initialize();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _RenderPipeline = new RenderPipeline(spriteBatch, GraphicsDevice, graphics);
            _MultiformManager = new MultiformManager();

            _Mouse = new MouseInput();
            _Keyboard = new KeyboardInput();
        }

        /// <summary>
        /// Required override. Initializes global AssetLoader.
        /// </summary>
        sealed protected override void LoadContent() 
        {
            // Supply the AssetLoader with the game's ContentManager object, so it
            // can actually load content.
            AssetLoader.Initialize(Content);
        }

        /// <summary>
        /// Required override.
        /// </summary>
        sealed protected override void UnloadContent() { }

        /// <summary>
        /// Required override. The main game loop.
        /// </summary>
        /// <param name="gameTime"></param>
        sealed protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _MultiformManager.Update();
            _Mouse.Update();
            _Keyboard.Update();
        }

        /// <summary>
        /// Required override. The main rendering loop, which gets called after Update.
        /// </summary>
        /// <param name="gameTime"></param>
        sealed protected override void Draw(GameTime gameTime)
        {
            _RenderPipeline.BegunRenderCycle = true;

            _MultiformManager.Render();

            _RenderPipeline.BegunRenderCycle = false;

            base.Draw(gameTime);
        }

        /// <summary>
        /// Register Multiform classes to the engine's MultiformManager.
        /// </summary>
        /// <param name="multiforms"></param>
        private void _RegisterMultiforms(Type[] multiforms)
        {
            foreach (var multiformType in multiforms)
            {
                // Get the multiform's name.
                string name = MultiformManager.GetMultiformName(multiformType);

                // Check if it's actually a multiform subtype.
                if (!multiformType.IsSubclassOf(typeof(Multiform)))
                {
                    throw new MultiformRegistrationException(
                        String.Format(
                            "Multiform with name {0} is not a subclass of `Multiform`.",
                            name)
                        );
                }

                var multiformInstance = (Multiform)Activator.CreateInstance(multiformType);
                MultiformManager.RegisterMultiform(name, multiformInstance);
            }
        }
    }
}
