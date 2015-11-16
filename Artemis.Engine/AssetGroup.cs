using Artemis.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Artemis.Engine
{
    public class AssetGroup : IDisposable
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
                return Parent == null ? Name
                                      : Parent.FullName + AssetLoader.ASSET_URI_SEPARATOR + Name;
            }
        }

        /// <summary>
        /// The parent of this asset group (if it exists).
        /// </summary>
        public AssetGroup Parent { get; private set; }

        /// <summary>
        /// Check if this group is empty (i.e. it has no assets and it has no
        /// subgroups).
        /// </summary>
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

        internal AssetGroup( string pathName
                           , SearchOption option
                           , string fileSearchQuery   = "*"
                           , string folderSearchQuery = "*"
                           , bool pruneEmptySubgroups = true )
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
                var assetName = Path.GetFileNameWithoutExtension(
                    DirectoryUtils.MakeRelativePath(
                        AssetLoader.ContentFolderName, fileName));

                Assets.Add(assetName, AssetLoader.LoadAssetUsingExtension(fileName));
            }

            // Prune empty subgroups.
            // 
            // ...
            //
            // I really have no good reason why anyone would not want to do this.

            if (pruneEmptySubgroups)
            {
                Subgroups = Subgroups.Where(kvp => !kvp.Value.IsEmpty)
                                 .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
        }

        /// <summary>
        /// Return a subgroup of this asset group with the given full name.
        /// 
        /// Example:
        /// If this group's name is "Parent", then calling GetSubgroup("Child.GrandChild")
        /// will return the group "Parent.Child.GrandChild".
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public AssetGroup GetSubgroup(string fullName)
        {
            return GetSubgroup(fullName.Split(AssetLoader.ASSET_URI_SEPARATOR));
        }

        internal AssetGroup GetSubgroup(string[] subgroupNameParts)
        {
            if (subgroupNameParts.Length == 1)
            {
                return Subgroups[subgroupNameParts[0]];
            }
            var newParts = subgroupNameParts.Skip(1).ToArray();
            return Subgroups[subgroupNameParts[0]].GetSubgroup(newParts);
        }

        /// <summary>
        /// Return the asset with the given full name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public T GetAsset<T>(string fullName)
        {
            var asset = GetAsset(fullName.Split(AssetLoader.ASSET_URI_SEPARATOR));
            if (asset is LazyAsset)
            {
                return ((LazyAsset)asset).Load<T>();
            }
            return (T)asset;
        }

        private object GetAsset(string[] nameParts)
        {
            if (nameParts.Length == 1)
            {
                return Assets[nameParts[0]];
            }
            var newParts = nameParts.Skip(1).ToArray();
            return Subgroups[nameParts[0]].GetAsset(newParts);
        }

        /// <summary>
        /// Remove a subgroup with the given full asset URI (without the
        /// parent group name).
        /// </summary>
        /// <param name="fullName"></param>
        public void RemoveSubgroup(string fullName)
        {
            RemoveSubgroup(fullName.Split(AssetLoader.ASSET_URI_SEPARATOR));
        }

        private void RemoveSubgroup(string[] nameParts)
        {
            if (nameParts.Length == 1)
            {
                var name = nameParts[0];

                Subgroups[name].Dispose();

                Subgroups.Remove(name);
            }
            var newParts = nameParts.Skip(1).ToArray();
            Subgroups[nameParts[0]].RemoveSubgroup(newParts);
        }

        private bool disposed = false;

        ~AssetGroup()
        {
            Dispose();
        }

        /// <summary>
        /// Clean up and dispose this AssetGroup object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Clean up and dispose this AssetGroup object.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Recursively dispose subgroups.
                    foreach (var group in Subgroups)
                    {
                        group.Value.Dispose(disposing);
                    }

                    // Dispose managed disposable asset types.
                    var IDisposableType = typeof(IDisposable);
                    foreach (var asset in Assets)
                    {
                        if (IDisposableType.IsAssignableFrom(asset.Value.GetType()))
                        {
                            var disposableAsset = (IDisposable)asset.Value;
                            disposableAsset.Dispose();
                        }
                    }
                }

                // Dispose native resources.
                Subgroups = null;
                Assets = null;

                disposed = true;
            }
        }
    }
}
