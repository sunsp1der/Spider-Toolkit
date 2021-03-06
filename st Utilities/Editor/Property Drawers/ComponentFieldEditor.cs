using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[CustomPropertyDrawer(typeof(ComponentField))]
public class ComponentFieldEditor : PropertyDrawer {

	Color boxColor;

	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label) {
		label = EditorGUI.BeginProperty(position, label, property);
		UnityEngine.Object comp = (UnityEngine.Object)Introspector.GetParent(property); 
		ComponentField componentField = (ComponentField)Introspector.GetValue(comp, property.name);
		EditorGUI.HelpBox(position,"",MessageType.None);

		// label
		position.height = 16;
		EditorGUI.PrefixLabel(position, label);

		// component
		Component originalComponent = componentField.component;
		EditorGUI.indentLevel += 1;
		position.y += 16;
		int buttonWidth = 50;
		Rect tempPosition = position;
		tempPosition.width -= buttonWidth + 3;

		EditorGUI.PropertyField( tempPosition, property.FindPropertyRelative("component"));

		// button
		tempPosition.x = tempPosition.x + tempPosition.width + 3;
		tempPosition.width = buttonWidth;
		bool copiedComponent =  stEditor.clipboard is Component;
		var tooltip = ( copiedComponent ) ? stEditor.clipboard.ToString() : 
			                                "Use Get Component\nin component menu";
		var button = new GUIContent( "Put", tooltip );
		if( GUI.Button( tempPosition, button) && copiedComponent)
		{
			Undo.RecordObject( comp, "Set ComponentField '"+property.name+"' Component");
			componentField.component = stEditor.clipboard;
		}
		GUI.enabled = true;

		//member
		if (componentField.component != originalComponent) {
			componentField.member = "";
		}
		position.y += 16;
		if ( componentField.component == null) {
			EditorGUI.Popup(position, "Member", 0, new string[]{});
		}
		else {
			var availableMembers = Introspector
								.GetSmartMemberList( componentField.component );
			int memberIndex = availableMembers.ToList().FindIndex(member => member == componentField.member);
			int selected = EditorGUI.Popup(position, "Member", memberIndex, availableMembers );
			if (selected >= 0) {
				Undo.RecordObject( comp, "Set ComponentField '"+property.name+"' Member");
				componentField.member = availableMembers[selected];
			}
		}
//EditorGUI.PropertyField( position, property.FindPropertyRelative("member") );
		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight ( SerializedProperty property, GUIContent label){
		return 48;
	}

}
