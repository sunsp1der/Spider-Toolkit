using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class Introspector {
#if UNITY_EDITOR
	public static object GetParent(SerializedProperty prop)
	{
		var path = prop.propertyPath.Replace(".Array.data[", "[");
		object obj = prop.serializedObject.targetObject;
		var elements = path.Split('.');
		foreach(var element in elements.Take(elements.Length-1))
		{
			if(element.Contains("["))
			{
				var elementName = element.Substring(0, element.IndexOf("["));
				var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[","").Replace("]",""));
				obj = GetIndexedValue(obj, elementName, index);
			}
			else
			{
				obj = GetValue(obj, element);
			}
		}
		return obj;
	}
#endif
	/// <summary>
	/// Gets a method from any object.
	/// </summary>
	/// <returns>The method.</returns>
	/// <param name="source">Source object</param>
	/// <param name="name">Name of method</param>
	public static MethodInfo GetMethod(object source, string name)
	{
		if(source == null)
			return null;
		var type = source.GetType();
		MethodInfo m = type.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		return  m;
	}

	/// <summary>
	/// Gets a field value from any object.
	/// </summary>
	/// <returns>The value.</returns>
	/// <param name="source">Source object</param>
	/// <param name="name">Name of field or property</param>
	public static object GetValue(object source, string name)
	{
		if(source == null)
			return null;
		var type = source.GetType();
		FieldInfo f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		if(f == null)
		{
			var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
			if(p == null)
				return null;
			return p.GetValue(source, null);
		}
		return f.GetValue(source);
	}

	/// <summary>
	/// Gets the value at a given index of an enumerable field of any object.
	/// </summary>
	/// <returns>The value at the given index</returns>
	/// <param name="source">source object</param>
	/// <param name="name">Name of field</param>
	/// <param name="index">Index of field</param>
	public static object GetIndexedValue(object source, string name, int index)
	{
		var enumerable = GetValue(source, name) as IEnumerable;
		var enm = enumerable.GetEnumerator();
		while(index-- >= 0)
			enm.MoveNext();
		return enm.Current;
	}

	/// <summary>
	/// Sets the value of a named field.
	/// </summary>
	/// <param name="source">Source object</param>
	/// <param name="name">Name of field</param>
	/// <param name="newValue">New value for field</param>
	public static void SetValue(object source, string name, object newValue)
	{
		FieldInfo info = source.GetType().GetField(name);
		if (info != null) {
			info.SetValue(source, newValue);
		}
	}

	/// <summary>
	/// Gets a list of fields and properties of a given component.
	/// </summary>
	/// <returns>The field list.</returns>
	/// <param name="component">The component</param>
	public static MemberInfo[] GetFieldList( Component component )
	{

		var baseMembers = component
			.GetType()
				.GetMembers( BindingFlags.Public | BindingFlags.Instance )
				.Where( m =>
				       (
					m.MemberType == MemberTypes.Field ||
					m.MemberType == MemberTypes.Property
					) &&
				       m.DeclaringType != typeof( MonoBehaviour ) &&
				       m.DeclaringType != typeof( Behaviour ) &&
				       m.DeclaringType != typeof( Component ) &&
				       m.DeclaringType != typeof( UnityEngine.Object )
				       )
				.OrderBy( m => m.Name )
				.ToArray();
		
		return baseMembers;
		
	}
}
/*
public static T GetReference<T>(object inObj, string fieldName) where T : class
{
	return GetField(inObj, fieldName) as T;
}

public static T GetValue<T>(object inObj, string fieldName) where T : struct
{
	return (T)GetField(inObj, fieldName);
}

*/