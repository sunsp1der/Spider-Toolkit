using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(Sound))]
public class SoundEditor : PropertyDrawer {

	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label) {
		object component = Introspector.GetParent(property); 
		Sound s = (Sound)Introspector.GetValue(component, property.name);

		label = EditorGUI.BeginProperty(position, label, property);
		position.height = 16;
		Rect contentPosition = EditorGUI.PrefixLabel(position, label);
		Rect buttonPosition = position;
		buttonPosition.x = contentPosition.x - 40;
		buttonPosition.width = 39;
		if ( /*EditorApplication.isPlaying &&*/ GUI.Button( buttonPosition, "Play")){
			if (s.mySource != null && s.mySource.isPlaying) {
				s.Stop ();
			}
			else {
				s.Play ();
			}
		}
		EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("audioClip"), GUIContent.none);
		if (s.foldout = EditorGUI.Foldout(position, s.foldout, GUIContent.none, true)) {
			EditorGUI.indentLevel = 1;
			position.y += 18;
			EditorGUI.PropertyField ( position, property.FindPropertyRelative("loop"));
			position.y += 18;
			EditorGUI.PropertyField( position, property.FindPropertyRelative("volume"));
			position.y += 18;
			EditorGUI.PropertyField ( position, property.FindPropertyRelative("pan"));
			position.y += 18;
			EditorGUI.PropertyField ( position, property.FindPropertyRelative("pitch"));
		}
		EditorGUI.EndProperty();
	}
	
	public override float GetPropertyHeight ( SerializedProperty property, GUIContent label){
		object component = Introspector.GetParent(property); 
		Sound s = (Sound)Introspector.GetValue(component, property.name);
		return s.foldout ? 90 : 18;
	}
}
