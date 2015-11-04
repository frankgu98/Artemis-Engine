using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Artemis.Engine
{
    /// <summary>
    /// The main Engine class, from which the game is setup and run.
    /// </summary>
    public sealed class Engine : Game
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

            MultiformManager.Update();
        }

        /// <summary>
        /// Required override. The main rendering loop, which gets called after Update.
        /// </summary>
        /// <param name="gameTime"></param>
        sealed protected override void Draw(GameTime gameTime)
        {
            _RenderPipeline.BeginRenderCycle();

            MultiformManager.Render();

            RenderPipeline.EndRenderCycle();

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

                var multiformInstance = Activator.CreateInstance(multiformType);
                MultiformManager.RegisterMultiform(name, multiformInstance);
            }
        }

        /// <summary>
        /// The singleton instance of Engine.
        /// </summary>
        private static Engine Instance;

        /// <summary>
        /// Whether or not Engine.Setup has been called.
        /// </summary>
        public static bool SetupCalled { get { return Instance != null; } }

        /// <summary>
        /// The engine's global render pipeline. Controls all rendering that takes
        /// place in the game.
        /// </summary>
        public static RenderPipeline RenderPipeline { get { return Instance._RenderPipeline; } }

        /// <summary>
        /// The engine's global multiform manager.
        /// </summary>
        public static MultiformManager MultiformManager { get { return Instance._MultiformManager; } }

        /// <summary>
        /// The global game properties.
        /// </summary>
        public static GameProperties GameProperties { get { return Instance._GameProperties; } }

        /// <summary>
        /// Setup the game's properties using the setup file with the supplied name.
        /// </summary>
        /// <param name="name"></param>
        public static void Setup(string name)
        {
            if (SetupCalled)
            {
                throw new EngineSetupException("Engine.Setup called multiple times.");
            }
            Instance = new Engine(new GameSetupReader(name).Read());
        }

        /// <summary>
        /// Setup the game's properties using the given setup parameters.
        /// </summary>
        public static void Setup(
            Resolution? baseResolution = null,
            bool fullscreen = GameProperties.DEFAULT_FULLSCREEN,
            bool fullscreenTogglable = GameProperties.DEFAULT_FULLSCREEN_TOGGLABLE,
            bool mouseVisible = GameProperties.DEFAULT_MOUSE_VISIBLE,
            bool mouseVisibilityTogglable = GameProperties.DEFAULT_MOUSE_VISIBILITY_TOGGLABLE,
            bool borderless = GameProperties.DEFAULT_BORDERLESS,
            bool borderTogglable = GameProperties.DEFAULT_BORDER_TOGGLABLE,
            bool vsync = GameProperties.DEFAULT_VSYNC,
            Color? bgColour = null,
            string? windowTitle = null)
        {
            var properties = new GameProperties();

            properties.BaseResolution = baseResolution.HasValue ? 
                baseResolution.Value : GameProperties.DEFAULT_RESOLUTION;

            properties.BackgroundColour = bgColour.HasValue ?
                bgColour.Value : GameProperties.DEFAULT_BG_COLOUR;

            if (windowTitle.HasValue)
            {
                properties.WindowTitle = windowTitle.Value;
            }

            properties.Fullscreen = fullscreen;
            properties.FullscreenTogglable = fullscreenTogglable;
            properties.MouseVisible = mouseVisible;
            properties.MouseVisibilityTogglable = mouseVisibilityTogglable;
            properties.Borderless = borderless;
            properties.BorderTogglable = borderTogglable;
            properties.VSync = vsync;

            Instance = new Engine(properties);
        }

        /// <summary>
        /// Register multiforms to the engine.
        /// </summary>
        /// <param name="multiforms"></param>
        public static void RegisterMultiforms(params Type[] multiforms)
        {
            if (!SetupCalled)
            {
                throw new EngineSetupException(
                    "Must call Engine.Setup before call to Engine.RegisterMultiforms.");
            }
            Instance._RegisterMultiforms(multiforms);
        }

        /// <summary>
        /// Indicate what multiform to construct upon game startup.
        /// </summary>
        /// <param name="multiform"></param>
        public static void StartWith(Type multiform)
        {
            if (!SetupCalled)
            {
                throw new EngineSetupException(
                    "Must call Engine.Setup before call to Engine.StartWith.");
            }
            MultiformManager.Construct(multiform);
        }

        /// <summary>
        /// Run the game.
        /// </summary>
        public static void Begin()
        {
            if (!SetupCalled)
            {
                throw new EngineSetupException(
                    "Must call Engine.Setup before call to Engine.Begin.");
            }
            Instance.Run();
        }

        /// <summary>
        /// End the game.
        /// </summary>
        public static void End()
        {

        }

    }
}
