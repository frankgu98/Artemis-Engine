using Artemis.Engine.Input;
using Microsoft.Xna.Framework;
using System;

namespace Artemis.Engine
{

    /// <summary>
    /// The part of the Engine object which is publically available to users.
    /// </summary>
    public sealed partial class Engine
    {
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
        /// The global game timer which records total elapsed game time, frames passed, 
        /// and elapsed time since the last update.
        /// </summary>
        public static GlobalTimer GameTimer { get { return Instance._GameTimer; } }

        /// <summary>
        /// The global game updater which is in charge of remembering and
        /// updating ArtemisObjects.
        /// </summary>
        public static GlobalUpdater GameUpdater { get { return Instance._GameUpdater; } }

        /// <summary>
        /// The global game mouse input provider.
        /// </summary>
        public static MouseInput Mouse { get { return Instance._Mouse; } }

        /// <summary>
        /// The global game keyboard input provider.
        /// </summary>
        public static KeyboardInput Keyboard { get { return Instance._Keyboard; } }

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
        public static void Setup( Resolution? baseResolution    = null
                                , bool fullscreen               = GameProperties.DEFAULT_FULLSCREEN
                                , bool fullscreenTogglable      = GameProperties.DEFAULT_FULLSCREEN_TOGGLABLE
                                , bool mouseVisible             = GameProperties.DEFAULT_MOUSE_VISIBLE
                                , bool mouseVisibilityTogglable = GameProperties.DEFAULT_MOUSE_VISIBILITY_TOGGLABLE
                                , bool borderless               = GameProperties.DEFAULT_BORDERLESS
                                , bool borderTogglable          = GameProperties.DEFAULT_BORDER_TOGGLABLE
                                , bool vsync                    = GameProperties.DEFAULT_VSYNC
                                , Color? bgColour               = null
                                , string? windowTitle           = null )
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

            using (Instance)
            {
                Instance.Run();
            }
        }
    }
}
