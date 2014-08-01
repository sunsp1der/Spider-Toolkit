using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(DictionaryStartValue))]

public class DictionaryStartValueEditor : Editor {

	public override void OnInspectorGUI()
	{
		DictionaryStartValue myTarget = (DictionaryStartValue)target;
		myTarget.dictionary = EditorGUILayout.TextField("Dictionary", myTarget.dictionary);
		myTarget.key = EditorGUILayout.TextField("Key", myTarget.key);
		myTarget.valueType = (DictionaryStartValue.valType)EditorGUILayout.EnumPopup("Value Type", myTarget.valueType);
		switch (myTarget.valueType) {
		case DictionaryStartValue.valType.String:
			myTarget.stringValue = EditorGUILayout.TextField("Value",myTarget.stringValue);
			break;
		case DictionaryStartValue.valType.Integer:
			myTarget.intValue = EditorGUILayout.IntField("Value", myTarget.intValue);
			break;
		case DictionaryStartValue.valType.Float:
			myTarget.floatValue = EditorGUILayout.FloatField("Value", myTarget.floatValue);
			break;
		case DictionaryStartValue.valType.GameObject:
			myTarget.gameObjectValue = (GameObject)EditorGUILayout.ObjectField("Value", myTarget.gameObjectValue, typeof(GameObject), true);
			break;
		case DictionaryStartValue.valType.Component:
			myTarget.componentValue = (Behaviour)EditorGUILayout.ObjectField("Value", myTarget.componentValue, typeof(Behaviour), true);
			break;
		}
		myTarget.overrideValue = EditorGUILayout.Toggle("Override Value", myTarget.overrideValue);
	}


}
