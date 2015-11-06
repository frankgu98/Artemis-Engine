using System;
using System.Collections.Generic;

namespace Artemis.Engine.Utilities
{

    /// <summary>
    /// A static class containing convenience methods for certain reflection
    /// related operations, such as retrieving attributes from an object.
    /// </summary>
    internal static class Reflection
    {

        /// <summary>
        /// Return the first Attribute of the given type applied to the give object.
        /// </summary>
        /// <param name="type">The object from which to retrieve the attribute.</param>
        /// <param name="attr">The attribute type.</param>
        /// <returns></returns>
    	public static Attribute GetFirstAttribute(Type type, Type attr)
    	{
    		foreach (var other_attr in Attribute.GetCustomAttributes(type))
    		{
    			if (other_attr.GetType() == attr)
    			{
    				return other_attr;
    			}
    		}
    		return null;
    	}

        /// <summary>
        /// Return a list of the attributes of the given type applied to the given object.
        /// </summary>
        /// <param name="type">The object from which to retrieve the attributes.</param>
        /// <param name="attr">The attribute type.</param>
        /// <returns></returns>
    	public static List<Attribute> GetAttributes(Type type, Type attr)
    	{
    		var attrs = new List<Attribute>();
    		foreach (var other_attr in Attribute.GetCustomAttributes(type))
    		{
    			if (other_attr.GetType() == attr)
    			{
    				attrs.Add(other_attr);
    			}
    		}
    		return attrs;
    	}

        /// <summary>
        /// Return the first Attribute of the given type applied to the given object.
        /// </summary>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <param name="type">The object from which to retrieve the attribute.</param>
        /// <returns></returns>
    	public static T GetFirstAttribute<T>(Type type) where T : Attribute
    	{
    		var attr = typeof(T);
    		foreach (var other_attr in Attribute.GetCustomAttributes(type))
    		{
    			if (other_attr.GetType() == attr)
    			{
    				return (T)other_attr;
    			}
    		}
    		return null;
    	}

        /// <summary>
        /// Return a list of attributes of the given type applied to the given object.
        /// </summary>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <param name="type">The object from which to retrieve the attributes.</param>
        /// <returns></returns>
    	public static List<T> GetAttributes<T>(Type type) where T : Attribute
    	{
            var attr = typeof(T);
    		var attrs = new List<T>();
    		foreach (var other_attr in Attribute.GetCustomAttributes(type))
    		{
    			if (other_attr.GetType() == attr)
    			{
    				attrs.Add((T)other_attr);
    			}
    		}
    		return attrs;
    	}

        /// <summary>
        /// Return the number of items defined in an enum.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int EnumItemCount(Type type)
        {
            return Enum.GetNames(type).Length;
        }

        /// <summary>
        /// Return all the items in an enum as a list.
        /// WARNING: This method isn't very fast, use with care!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> EnumItems<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("Generic parameter must be an enum.");
            }
            return new List<T>((T[])Enum.GetValues(typeof(T)));
            
        }
    }
}
