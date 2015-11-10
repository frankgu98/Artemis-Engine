using Artemis.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Artemis.Engine
{
    public class AssetGroup
    {

        /// <summary>
        /// The unqualified name of this asset group.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The full name of this asset group all the way up the asset group tree.
        /// </summary>
        public string FullName
        {
            get
            {
                return Parent == null ? Name : Parent.FullName + "." + Name;
            }
        }

        /// <summary>
        /// The parent of this asset group (if it exists).
        /// </summary>
        public AssetGroup Parent { get; private set; }

        public bool IsEmpty
        {
            get
            {
                return (Assets.Count == 0 && Subgroups.Count == 0);
            }
        }

        /// <summary>
        /// The dictionary of subgroups of this group, mapping from unqualified 
        /// names to associated subgroups.
        /// </summary>
        private Dictionary<string, AssetGroup> Subgroups = new Dictionary<string, AssetGroup>();

        /// <summary>
        /// The dictionary of all assets in this group.
        /// </summary>
        private Dictionary<string, object> Assets = new Dictionary<string, object>();

        internal AssetGroup(
            string pathName, SearchOption option, 
            string fileSearchQuery = "*", string folderSearchQuery = "*")
        {
            Name = Path.GetFileName(pathName);

            // Search for directories to turn into subgroups.
            if (option == SearchOption.AllDirectories)
            {
                var directories = Directory.EnumerateDirectories(
                    pathName, folderSearchQuery, SearchOption.TopDirectoryOnly);

                foreach (var folderName in directories)
                {
                    var subgroup = new AssetGroup(folderName, option, fileSearchQuery, folderSearchQuery);

                    if (subgroup.IsEmpty)
                    {
                        continue;
                    }

                    subgroup.Parent = this;
                    Subgroups.Add(subgroup.Name, subgroup);
                }
            }
            // Otherwise, option == SearchOption.TopDirectoryOnly, and we only have to look for
            // asset files in the given directory rather than subdirectories as well.

            var files = Directory.EnumerateFiles(
                pathName, fileSearchQuery, SearchOption.TopDirectoryOnly);

            foreach (var fileName in files)
            {
                var assetFileName = DirectoryUtils.MakeRelativePath(
                    AssetLoader.ContentFolderName, fileName);
                var assetName = Path.GetFileName(assetFileName);

                Assets.Add(assetName, AssetLoader.LoadAssetUsingExtension(assetName));
            }

            // Prune empty subgroups.

            var toRemove = new List<string>();

            foreach (var kvp in Subgroups)
            {
                if (kvp.Value.IsEmpty)
                {
                    toRemove.Add(kvp.Key);
                }
            }

            foreach (var name in toRemove)
            {
                Subgroups.Remove(name);
            }

        }
    }
}
