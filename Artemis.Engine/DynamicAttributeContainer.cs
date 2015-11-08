using System;
using System.Collections.Generic;

namespace Artemis.Engine.Utilities
{
    public class DynamicAttributeContainer
    {
        /// <summary>
        /// List of attributs of and ArtemisObject
        /// </summary>
        internal Dictionary<string, Object> Attributes { get; private set; }

        public DynamicAttributeContainer()
        {
            Attributes = new Dictionary<string, Object>();
        }

        /// <summary>
        /// Get value of attribute
        /// </summary>
        public T Get<T>(string name)
        {
            return (T)Attributes[name];
        }

        /// <summary>
        /// Set the value of an attribute. If the attribute does not exist, it will be createed.
        /// </summary>
        public void Set<T>(string name, T obj)
        {
            if (Attributes.ContainsKey(name))
            {
                Attributes[name] = obj;
            }
            else
            {
                Attributes.Add(name, obj);
            }
        }
    }
}
