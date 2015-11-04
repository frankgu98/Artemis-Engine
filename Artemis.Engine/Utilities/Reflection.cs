using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Engine.Utilities
{
    internal class Reflection
    {

    	public static Attribute GetFirstAttribute(Type type, Type attr)
    	{
    		foreach (var other_attr in System.Attribute.GetCustomAttributes(type))
    		{
    			if (other_attr.GetType() == attr)
    			{
    				return other_attr;
    			}
    		}
    		return null;
    	}

    	public static List<Attribute> GetAttributes(Type type, Type attr)
    	{
    		var attrs = new List<Attribute>();
    		foreach (var other_attr in System.Attribute.GetCustomAttributes(type))
    		{
    			if (other_attr.GetType() == attr)
    			{
    				attrs.Add(other_attr);
    			}
    		}
    		return attrs;
    	}

    	public static T GetFirstAttribute<T>(Type type) where T : Attribute
    	{
    		var attr = typeof(T);
    		foreach (var other_attr in System.Attribute.GetCustomAttributes(type))
    		{
    			if (other_attr.GetType() == attr)
    			{
    				return (T)other_attr;
    			}
    		}
    		return null;
    	}

    	public static List<T> GetAttributes<T>(Type type) where T : Attribute
    	{
    		var attrs = new List<T>();
    		foreach (var other_attr in System.Attributes.GetCustomAttributes(type))
    		{
    			if (other_attr.GetType() == attr)
    			{
    				attrs.Add((T)other_attr);
    			}
    		}
    		return attrs;
    	}

    }
}
