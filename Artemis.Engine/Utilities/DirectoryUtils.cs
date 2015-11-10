using System;
using System.IO;

namespace Artemis.Engine.Utilities
{
    public static class DirectoryUtils
    {

        #region Code by Stackoverflow Users "Dave" and "Marc Gravell"
        // References: http://stackoverflow.com/a/340454
        //             http://stackoverflow.com/a/703292
    
        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        public static String MakeRelativePath(String rootDirectory, String childDirectory)
        {
            if (String.IsNullOrEmpty(rootDirectory))
            {
                throw new ArgumentNullException("rootDirectory");
            }

            if (String.IsNullOrEmpty(childDirectory))
            {
                throw new ArgumentNullException("childDirectory");
            }

            Uri rootUri = new Uri(rootDirectory);

            // Folders must end in a slash
            if (!childDirectory.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                childDirectory += Path.DirectorySeparatorChar;
            }
            Uri childUri = new Uri(childDirectory);

            return Uri.UnescapeDataString(
                childUri.MakeRelativeUri(rootUri)
                        .ToString()
                        .Replace('/', Path.DirectorySeparatorChar));
        }

        #endregion

    }
}
