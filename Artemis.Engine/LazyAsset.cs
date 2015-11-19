using Microsoft.Xna.Framework.Content;
using System;

namespace Artemis.Engine
{
    public sealed class LazyAsset
    {

        /// <summary>
        /// The name of the asset.
        /// </summary>
        public string AssetName { get; private set; }

        /// <summary>
        /// The memo of the asset.
        /// </summary>
        private object memo;

        /// <summary>
        /// The type of the asset memo.
        /// </summary>
        private Type assetType;

        public LazyAsset(string name)
        {
            AssetName = name;
        }

        /// <summary>
        /// Load the asset.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Load<T>()
        {
            if (memo == null && typeof(T) == assetType)
            {
                try
                {
                    memo = AssetLoader.Load<T>(AssetName);
                    assetType = typeof(T);
                }
                catch (ContentLoadException exception)
                {
                    throw new AssetLoadException(
                        String.Format(
                            "LazyAsset with path '{0}' could not be loaded as " +
                            "the given type ({0}).", AssetName, typeof(T)),
                        exception);
                }
            }
            return (T)memo;
        }

    }
}
