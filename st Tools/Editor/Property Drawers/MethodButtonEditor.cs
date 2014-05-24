using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

[CustomPropertyDrawer(typeof(MethodButton))]
public class MethodButtonEditor : PropertyDrawer {
	string name;
	
	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label) {
		if (property.name.Substring(0,1)!="_") {
			Debug.Log("MethodButton should be named _[name of method]");
		}
		object component = Introspector.GetParent(property); 
		string methodName = property.name.Substring(1);
		//var value = Introspector.GetValue(component, property.name.Substring(1));
		//		label = EditorGUI.BeginProperty(position, label, property);
		MethodButton methodButton = (MethodButton)Introspector.GetValue(component, property.name);
		if (methodButton != null) {
			label.tooltip = methodButton.tooltip;
		}
		Rect contentPosition = EditorGUI.PrefixLabel(position, label);
		if (GUI.Button(contentPosition,new GUIContent(methodName+"()", label.tooltip))){
			MethodInfo methodInfo = Introspector.GetMethod(component, methodName);
			if (methodInfo != null){
				methodInfo.Invoke( component, null);
			}
		}
	}
}