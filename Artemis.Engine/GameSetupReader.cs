using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace Artemis.Engine
{

    /// <summary>
    /// 
    /// </summary>
    internal class GameSetupReader
    {

        #region Xml Constants

        private const string BASE_RESOLUTION_ELEMENT = "BaseResolution";
        private const string FULLSCREEN_ELEMENT = "Fullscreen";
        private const string FULLSCREEN_TOGGLABLE_ELEMENT = "FullscreenTogglable";
        private const string MOUSE_VISIBLE_ELEMENT = "MouseVisible";
        private const string MOUSE_VISIBILITY_TOGGLABLE_ELEMENT = "MouseVisibilityTogglable";
        private const string BORDERLESS_ELEMENT = "Borderless";
        private const string BORDER_TOGGLABLE_ELEMENT = "BorderTogglable";
        private const string WINDOW_TITLE_ELEMENT = "WindowTitle";
        private const string VSYNC_ELEMENT = "VSync";
        private const string BG_COLOUR_ELEMENT = "BackgroundColour";
        private const string CONTENT_FOLDER_ELEMENT = "ContentFolder";

        private const string RESOLUTION_REGEX = @"[0-9]+x[0-9]+$";
        private const string COLOUR_REGEX = @"0(x|X)[0-9a-fA-F]{6}$";

        private const string NATIVE_RESOLUTION = "Native";

        #endregion

        /// <summary>
        /// The name of the setup file, relative to the application startup directory.
        /// </summary>
        public string SetupFileName { get; private set; }

        public GameSetupReader(string fileName)
        {
            SetupFileName = fileName;
        }

        /// <summary>
        /// Read the setup file and return a GameProperties object.
        /// </summary>
        /// <returns></returns>
        public GameProperties Read()
        {
            var properties = new GameProperties();
            var setupFile = new XmlDocument();
            setupFile.Load(SetupFileName);

            XmlElement root;
            try
            {
                root = setupFile.ChildNodes[1] as XmlElement;
            }
            catch (IndexOutOfRangeException)
            {
                // LOG: Could not load setup file, invalid Xml structure.
                return properties;
            }
            
            foreach (var node in root.ChildNodes)
            {
                var element = node as XmlElement;

                // Just continue if we don't know what we're parsing.
                if (element == null)
                {
                    continue;
                }

                ReadElementAsGameProperty(element, properties);       
            }

            return properties;
        }

        /// <summary>
        /// Read an XmlElement found in the document root and apply it
        /// to the GameProperties object.
        /// </summary>
        private void ReadElementAsGameProperty(XmlElement element, GameProperties properties)
        {
            switch (element.Name)
            {
                case BASE_RESOLUTION_ELEMENT:
                    properties.BaseResolution = ReadResolution(element, GameProperties.DEFAULT_RESOLUTION);
                    break;
                case FULLSCREEN_ELEMENT:
                    properties.Fullscreen = ReadBool(element, GameProperties.DEFAULT_FULLSCREEN);
                    break;
                case FULLSCREEN_TOGGLABLE_ELEMENT:
                    properties.FullscreenTogglable = ReadBool(element, GameProperties.DEFAULT_FULLSCREEN_TOGGLABLE);
                    break;
                case MOUSE_VISIBLE_ELEMENT:
                    properties.MouseVisible = ReadBool(element, GameProperties.DEFAULT_MOUSE_VISIBLE);
                    break;
                case MOUSE_VISIBILITY_TOGGLABLE_ELEMENT:
                    properties.MouseVisibilityTogglable = ReadBool(element, GameProperties.DEFAULT_MOUSE_VISIBILITY_TOGGLABLE);
                    break;
                case BORDERLESS_ELEMENT:
                    properties.Borderless = ReadBool(element, GameProperties.DEFAULT_BORDERLESS);
                    break;
                case BORDER_TOGGLABLE_ELEMENT:
                    properties.BorderTogglable = ReadBool(element, GameProperties.DEFAULT_BORDER_TOGGLABLE);
                    break;
                case WINDOW_TITLE_ELEMENT:
                    properties.WindowTitle = element.InnerText;
                    break;
                case VSYNC_ELEMENT:
                    properties.VSync = ReadBool(element, GameProperties.DEFAULT_VSYNC);
                    break;
                case BG_COLOUR_ELEMENT:
                    properties.BackgroundColour = ReadColour(element, GameProperties.DEFAULT_BG_COLOUR);
                    break;
                case CONTENT_FOLDER_ELEMENT:
                    properties.ContentFolder = element.InnerText;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Read a boolean value from an XmlElement.
        /// </summary>
        private bool ReadBool(XmlElement element, bool defaultValue)
        {
            var text = element.InnerText;
            bool val;
            try
            {
                val = Convert.ToBoolean(text);
            }
            catch (FormatException)
            {
                // Log that we couldn't get the value.
                return defaultValue;
            }
            return val;
        }

        /// <summary>
        /// Read a Resolution object from an XmlElement.
        /// </summary>
        private Resolution ReadResolution(XmlElement element, Resolution defaultValue)
        {
            var text = element.InnerText;
            if (text == NATIVE_RESOLUTION)
                return Resolution.Native;
            if (!Regex.IsMatch(text, RESOLUTION_REGEX))
            {
                // Log that we couldn't figure out the resolution.
                return defaultValue;
            }

            var parts = text.Split('x');

            var width = Int32.Parse(parts[0]);
            var height = Int32.Parse(parts[1]);
            return new Resolution(width, height);
        }

        /// <summary>
        /// Read a Color object from an XmlElement.
        /// </summary>
        private Color ReadColour(XmlElement element, Color defaultValue)
        {
            var text = element.InnerText;
                
            // If the given colour string is a name, attempt to find it as a static property
            // of Color.
            var prop = typeof(Color).GetProperty(
                    text,
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy
                    );
            if (prop != null)
            {
                return (Color)prop.GetValue(null, null);
            }

            if (!Regex.IsMatch(text, COLOUR_REGEX))
            {
                // Log that we couldn't figure out the colour.
                return defaultValue;
            }
            var val = Convert.ToInt32(text, 16);
            var r = 0xff & (val >> 16);
            var g = 0xff & (val >> 8);
            var b = 0xff & val;
            return new Color(r, g, b);
        }

    }
}
