using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(LockedView))]
public class LockedViewEditor : PropertyDrawer {
	string name;

	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label) {
		if (property.name.Substring(0,1)!="_") {
			Debug.Log("LockedView should be named _[name of viewed member]");
		}
		object component = Introspector.GetParent(property); 
		string fieldName = property.name.Substring(1);
		var value = Introspector.GetValue(component, fieldName);
		//var value = Introspector.GetValue(component, property.name.Substring(1));
		//label = EditorGUI.BeginProperty(position, label, property);
		LockedView lockedView = (LockedView)Introspector.GetValue(component, property.name);
		if (lockedView != null){
			label.tooltip = lockedView.tooltip;
		}
		Rect contentPosition = EditorGUI.PrefixLabel(position, label);
		EditorGUI.LabelField ( contentPosition, (value==null)?"Null":value.ToString());
		EditorUtility.SetDirty( (UnityEngine.Object)component);
	}
}