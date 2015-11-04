using Microsoft.Xna.Framework;

namespace Artemis.Engine
{
    public class GameProperties
    {

        #region Default Values

        internal static readonly Resolution DEFAULT_RESOLUTION = new Resolution(800, 600);
        internal static readonly bool DEFAULT_FULLSCREEN = false;
        internal static readonly bool DEFAULT_FULLSCREEN_TOGGLABLE = false;
        internal static readonly bool DEFAULT_MOUSE_VISIBLE = true;
        internal static readonly bool DEFAULT_MOUSE_VISIBILITY_TOGGLABLE = false;
        internal static readonly bool DEFAULT_BORDERLESS = false;
        internal static readonly bool DEFAULT_BORDER_TOGGLABLE = false;
        internal static readonly bool DEFAULT_VSYNC = false;
        internal static readonly Color DEFAULT_BG_COLOUR = Color.Black;
        internal static readonly string DEFAULT_CONTENT_FOLDER = "Content";

        #endregion

        /// <summary>
        /// The game's base resolution. The base resolution acts as a "default" resolution,
        /// from which every renderable item gets scaled relative to when the resolution changes.
        /// </summary>
        public Resolution BaseResolution { get; internal set; }

        /// <summary>
        /// Whether or not the game is fullscreen on startup.
        /// </summary>
        public bool Fullscreen { get; internal set; }

        /// <summary>
        /// Whether or not fullscreen is togglable.
        /// </summary>
        public bool FullscreenTogglable { get; internal set; }

        /// <summary>
        /// Whether or not the mouse is visible on startup.
        /// </summary>
        public bool MouseVisible { get; internal set; }

        /// <summary>
        /// Whether or not mouse visibility is togglable.
        /// </summary>
        public bool MouseVisibilityTogglable { get; internal set; }

        /// <summary>
        /// Whether or not the window is borderless on startup.
        /// </summary>
        public bool Borderless { get; internal set; }

        /// <summary>
        /// Whether or not the window border is togglable.
        /// </summary>
        public bool BorderTogglable { get; set; }

        /// <summary>
        /// The window title.
        /// </summary>
        public string WindowTitle { get; set; }

        /// <summary>
        /// Whether or not the game's renderer uses vertical synchronization.
        /// </summary>
        public bool VSync { get; internal set; }

        /// <summary>
        /// The background colour.
        /// </summary>
        public Color BackgroundColour { get; internal set; }

        /// <summary>
        /// The content folder name.
        /// </summary>
        public string ContentFolder { get; internal set; }

        internal GameProperties()
        {
            // Setup default values.
            BaseResolution           = DEFAULT_RESOLUTION;
            Fullscreen               = DEFAULT_FULLSCREEN;
            FullscreenTogglable      = DEFAULT_FULLSCREEN_TOGGLABLE;
            MouseVisible             = DEFAULT_MOUSE_VISIBLE;
            MouseVisibilityTogglable = DEFAULT_MOUSE_VISIBILITY_TOGGLABLE;
            Borderless               = DEFAULT_BORDERLESS;
            BorderTogglable          = DEFAULT_BORDER_TOGGLABLE;
            VSync                    = DEFAULT_VSYNC;
            BackgroundColour         = DEFAULT_BG_COLOUR;
            ContentFolder            = DEFAULT_CONTENT_FOLDER;
        }

    }
}
