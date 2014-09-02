using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class Introspector {

	public static Dictionary<Type, string[]> StructTypes = new Dictionary<Type, string[] > {
		{typeof(Vector2), new string[]{"x","y"}},
		{typeof(Vector3), new string[]{"x","y","z"}},
		{typeof(Color), new string[]{"r","g","b","a"}},
		{typeof(Color32), new string[]{"r","g","b","a"}},
		{typeof(Quaternion), new string[]{"z"}}
	};

	/// <summary>
	/// Gets a smart member list. Smart member list is a list of properties, fields AND
	/// important subproperties of certain types including simple classes, quaternions,
	/// colors, and vectors.
	/// </summary>
	/// <returns>The smart property list.</returns>
	/// <param name="component">Component.</param>
	public static string[] GetSmartMemberList( Component component) {
		List<string> propertyList = new List<string>();
		Type componentType = component.GetType ();
		// get all field and property info
		foreach (MemberInfo m in componentType.GetMembers( BindingFlags.Public |
		                                                   BindingFlags.Instance)) {

			if (// skip if it's not a field or a property
				(m.MemberType != MemberTypes.Field && 
			     m.MemberType != MemberTypes.Property) ||
			    // skip if it's hidden in the inspector 
			    Attribute.IsDefined(m, typeof(HideInInspector)) || 
			    // skip if it's a field/property of our parent classes
				m.DeclaringType == typeof( MonoBehaviour ) ||
			    m.DeclaringType == typeof( Behaviour ) ||
			    m.DeclaringType == typeof( Component ) ||
			    m.DeclaringType == typeof( UnityEngine.Object )
			    ) {
				continue;
			}
			// get data type
			Type dataType;
			if (m.MemberType == MemberTypes.Field) {
				dataType = componentType.GetField(m.Name).FieldType;
			}
			else {
				dataType = componentType.GetProperty(m.Name).PropertyType;
			}
			// skip special st Types
			if (dataType == typeof(LockedView) || dataType == typeof(MethodButton)) {
				continue;
			}

			// add smart member names
			bool hasSubFields = false; // true if it's a special type with subfields
			// check if it's a serializable class declared in our component (or its parents)
			if (dataType.IsClass && dataType.DeclaringType != null && dataType.IsSerializable &&
			    			dataType.DeclaringType.IsAssignableFrom(componentType)) {
				hasSubFields = true;
				// show subfields
				foreach (MemberInfo subMember in dataType.GetMembers( 
				                                                  BindingFlags.Public |
				                                                  BindingFlags.Instance)) {			
					// skip if it's not a field or a property
					if (subMember.MemberType != MemberTypes.Field && 
					    				subMember.MemberType != MemberTypes.Property) {
						continue;
					}
					// get type
					Type subMemberType;
					if (m.MemberType == MemberTypes.Field) {
						subMemberType = componentType.GetField(m.Name).FieldType;
					}
					else {
						subMemberType = componentType.GetProperty(m.Name).PropertyType;
					}
					bool hasSubSubFields = false; //!!
					// check against smart fields
					foreach (Type type in StructTypes.Keys) {
						if (subMemberType == type) {
							foreach (string subField in StructTypes[type]) {
								propertyList.Add( m.Name+"."+subMember.Name +"."+subField);
							}
							hasSubSubFields = true;
							break;
						}
					}
					if (!hasSubSubFields) {
						propertyList.Add( m.Name+"."+subMember.Name);
					}
				}
			}
			else {
				// check against smart fields
				foreach (Type type in StructTypes.Keys) {
					if (dataType == type) {
						foreach (string subField in StructTypes[type]) {
							propertyList.Add( m.Name + "." + subField);
						}
						hasSubFields = true;
						break;
					}
				}
			}
			if (!hasSubFields) {
				propertyList.Add( m.Name);
			}
		}
		return propertyList.ToArray();
	}

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
	/// Gets a field value from any object using SmartMember (allows subobjects).
	/// </summary>
	/// <returns>The value.</returns>
	/// <param name="source">Source object</param>
	/// <param name="name">SmartName of field or property</param>
	public static object GetSmartValue(object source, string name)
	{
		if(source == null)
			return null;
		object memberInfo = null;
		object actualSource = null;
		string actualName = "";
		GetSmartMemberInfo( source, name, ref memberInfo, ref actualSource, ref actualName);

		// hack Special treatment for quaternions
		if (actualSource.GetType() == typeof(Quaternion)) {
			return ((Quaternion)actualSource).eulerAngles.z;
		}
		// end hack

		if (memberInfo is FieldInfo) {
			return ((FieldInfo)memberInfo).GetValue(actualSource);
		}
		else {
			return ((PropertyInfo)memberInfo).GetValue(actualSource, null);
		}
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
		if (info == null) {
			PropertyInfo pInfo = source.GetType().GetProperty (name);
			if (pInfo != null) {
				pInfo.SetValue(source, newValue, null);
			}
		}
		if (info != null) {
			info.SetValue(source, newValue);
		}
	}

	/// <summary>
	/// Sets the value of a named field using SmartMember (allows for subobjects).
	/// </summary>
	/// <param name="source">Source object</param>
	/// <param name="name">Smart name of field</param>
	/// <param name="newValue">New value for field</param>
	public static void SetSmartValue(object source, string name, object newValue)
	{
		if (!name.Contains(".")) {
			SetValue(source, name, newValue);
		}
		else {
			object memberInfo = null;
			object actualSource = null;
			string actualName = "";
			// get information for setting the correct value, deciphering dot references
			GetSmartMemberInfo( source, name, ref memberInfo, ref actualSource, ref actualName);
			// check if it's one of our special struct types defined in Introspector.StructTypes
			if (StructTypes.ContainsKey(actualSource.GetType())) {
				// divide out the actual value and the property to be set
				int lastDot = name.LastIndexOf('.');
				string fieldName = name.Substring (lastDot + 1);
				string structName = name.Substring(0, lastDot);

				// hack Special treatment for quaternions
				if (actualSource.GetType() == typeof(Quaternion)) {
					Quaternion q = (Quaternion) actualSource;
					q = Quaternion.Euler (q.eulerAngles.x, q.eulerAngles.y, (float)newValue);
					SetSmartValue ( source, structName, q);
				}
				// end hack

				else {
					// Set the specific attribute of our struct value
					SetValue (actualSource, fieldName, newValue);
					SetSmartValue( source, structName, actualSource);
				}
			}
			// not a special struct type, just apply value to the object located by GetSmartMemberInfo
			else {
				if (memberInfo is FieldInfo) {
					((FieldInfo)memberInfo).SetValue(actualSource, newValue);
				}
				else {
					((PropertyInfo)memberInfo).SetValue(actualSource, newValue, null);
				}
			}
		}
	}

	public static void GetSmartMemberInfo(object source, string name, ref object memberInfo, 
	                                 		ref object actualSource, ref string actualName) {
		string[] parsedName = name.Split('.');
		Type sourceType = source.GetType();
		FieldInfo info = null;
		PropertyInfo pInfo = null;
		int i;
		for (i = 0; i < parsedName.Length; i++) {
			info = sourceType.GetField (parsedName[i]);
			if (info != null) {
				if (i<parsedName.Length-1) source = info.GetValue (source);
			}
			else {
				pInfo = sourceType.GetProperty(parsedName[i]);
				if (i<parsedName.Length-1) source = pInfo.GetValue (source, null);
			}
			sourceType = source.GetType();
		}
		actualSource = source;
		actualName = parsedName.Last();
		if (info != null) {
			memberInfo = info;
		}
		else {
			memberInfo = pInfo;
		}
	}

	/// <summary>
	/// Gets a list of fields and properties of a given component.
	/// </summary>
	/// <returns>The field list.</returns>
	/// <param name="component">The component</param>
	public static MemberInfo[] GetFieldList( Component component )
	{
		MemberInfo[] baseMembers = component
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
}
