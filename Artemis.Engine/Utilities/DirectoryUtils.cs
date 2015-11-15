using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Artemis.Engine.Utilities
{
    public static class DirectoryUtils
    {

        public static String MakeRelativePath(string rootDirectory, string childDirectory)
        {
            if (String.IsNullOrEmpty(rootDirectory))
            {
                throw new ArgumentNullException("rootDirectory");
            }
            if (String.IsNullOrEmpty(childDirectory))
            {
                throw new ArgumentNullException("childDirectory");
            }
            if (!IsChildDirectoryOf(rootDirectory, childDirectory))
            {
                throw new ArgumentException(
                    String.Format("`childDirectory` must be child of `rootDirectory`. " + 
                                  "'{0}' is not a child directory of '{1}'.", childDirectory, rootDirectory));
            }
            var parentComponents = rootDirectory.Split(Path.DirectorySeparatorChar);
            var childComponents  = childDirectory.Split(Path.DirectorySeparatorChar);

            Debug.Assert(parentComponents.Length < childComponents.Length);

            int i = 0;
            var maxLen = parentComponents.Length;
            while (i < maxLen && parentComponents[i] == childComponents[i]) i++;

            return String.Join(Path.DirectorySeparatorChar.ToString(), childComponents.Skip(i));
        }

        /// <summary>
        /// Determine if a given directory is a child directory of another.
        /// </summary>
        /// <param name="parentDirectory"></param>
        /// <param name="childDirectory"></param>
        /// <returns></returns>
        public static bool IsChildDirectoryOf(string parentDirectory, string childDirectory)
        {
            var childInfo  = new DirectoryInfo(childDirectory);
            var parentInfo = new DirectoryInfo(parentDirectory);

            while (childInfo.Parent != null)
            {
                if (childInfo.Parent.FullName == parentInfo.FullName)
                {
                    return true;
                }
                childInfo = childInfo.Parent;
            }
            return false;
        }

    }
}
