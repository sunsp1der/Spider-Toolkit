using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(stDictionary))]

public class stDictionaryEditor : Editor {

	bool expanded = false;

	public override void OnInspectorGUI()
	{
		stDictionary myTarget = (stDictionary)target;
		myTarget.dataName = EditorGUILayout.TextField("Data Name", myTarget.dataName);
		myTarget.networked = EditorGUILayout.Toggle("Networked", myTarget.networked);
		if (expanded = EditorGUILayout.Foldout( expanded, "Contents")){
			EditorGUI.indentLevel += 1;
			bool empty = true;
			string[] keys = myTarget.dict.Keys.ToArray();
			foreach( string key in keys) {
				empty = false;
				var data = myTarget.dict[key];
				if (data is GameObject) {
					myTarget.dict[key] = EditorGUILayout.ObjectField( key, (GameObject)data, typeof(GameObject), true);
				}
				else if (data is Behaviour) {
					myTarget.dict[key] = EditorGUILayout.ObjectField (key, (Behaviour)data, typeof(Behaviour), true);
				}
				else if (data == null) {
					EditorGUILayout.LabelField(key,"{NULL}");
				}
				else {
					switch (data.GetType().ToString()) {
					case "System.Int32":
						myTarget.dict[key] = EditorGUILayout.IntField( key, (int)data);
						break;
					case "System.Single":
						myTarget.dict[key] = EditorGUILayout.FloatField( key, (float)data);
						break;
					case "System.Double":
						float f=0;
						bool success = false;
						try {
							f = System.Convert.ToSingle(data);
							success = true;
						}
						catch (System.OverflowException) {
							EditorGUILayout.LabelField(key, data.ToString ());
						}
						if (success) {
							myTarget.dict[key] = (double)EditorGUILayout.FloatField( key, (float)f);
						}
						break;
					case "System.String":
						myTarget.dict[key] = EditorGUILayout.TextField( key, (string)data);
						break;
					default:
						EditorGUILayout.LabelField(key, data.ToString ());
						break;
					}
				}
			}
			if (empty) {
				EditorGUILayout.LabelField("{empty}");
			}
		}

	}


}
