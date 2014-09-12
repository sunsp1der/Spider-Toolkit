using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[CustomEditor(typeof(stDictionary))]

public class stDictionaryEditor : Editor {

	bool contentsExpanded = false;
	bool startValuesExpanded = false;

	public override void OnInspectorGUI()
	{
		stDictionary myTarget = (stDictionary)target;
		myTarget.dataName = EditorGUILayout.TextField("Data Name", myTarget.dataName);
		myTarget.networked = EditorGUILayout.Toggle("Networked", myTarget.networked);
		EditorGUI.indentLevel +=1;
		if (Application.isPlaying) {
			// show actual dictionary
			if (contentsExpanded = EditorGUILayout.Foldout( contentsExpanded, "Contents")){
				EditorGUI.indentLevel += 1;
				string[] keys = myTarget.dict.Keys.ToArray();
				if (keys.Length == 0) {
					EditorGUILayout.LabelField("{empty}");
				}
				foreach( string key in keys) {
					var data = myTarget.dict[key];
					if (data is GameObject) {
						myTarget.dict[key] = EditorGUILayout.ObjectField( key, (GameObject)data, typeof(GameObject), 
						                                                 true);
					}
					else if (data is string) {
						myTarget.dict[key] = EditorGUILayout.TextField( key, (string)data);
					}
					else if (data is Int32) {
						myTarget.dict[key] = EditorGUILayout.IntField( key, (int)data);
					}
					else if (data is Single) {
						myTarget.dict[key] = EditorGUILayout.FloatField( key, (float)data);
					}
					else if (data is Vector2) {
						myTarget.dict[key] = EditorGUILayout.Vector2Field( key, (Vector2)data);
					}
					else if (data is Vector3) {
						myTarget.dict[key] = EditorGUILayout.Vector3Field( key, (Vector3	)data);
					}
					else {
						EditorGUILayout.LabelField(key, data.ToString ()+" (Type "+data.GetType().ToString()+")");
					}
				}
			}
		}
		// show start values
		if (startValuesExpanded = EditorGUILayout.Foldout( startValuesExpanded, "Start Values")){
			Undo.RecordObject(myTarget, "Change Start Values");
			EditorGUI.indentLevel += 1;
			const int buttonWidth = 18;
			EditorGUILayout.BeginHorizontal();
			// column headers
			EditorGUILayout.LabelField(new GUIContent("Key","Empty or duplicate values will cause errors"),
			                           GUILayout.MinWidth(50));
			EditorGUILayout.LabelField(new GUIContent("Value","New entries must be created to change data type"),
			                           GUILayout.MinWidth(50));
			// new button 
			// HACK the buttonwidth should be *2 not *4, but enumpopup is being weird
			DictValue.Types newType = (DictValue.Types)EditorGUILayout.EnumPopup( DictValue.Types.New, 
						new GUILayoutOption[] { GUILayout.Width (buttonWidth*4), GUILayout.Height(15)});
			if (newType != DictValue.Types.New) {
				switch (newType){
				case DictValue.Types.New:
					break;
				case DictValue.Types.Int:
					myTarget.startValues.Insert (0, new DictValue((int)0));
					break;
				case DictValue.Types.Float:
					myTarget.startValues.Insert (0, new DictValue(0.0f));
					break;
				case DictValue.Types.String:
					myTarget.startValues.Insert (0, new DictValue(""));
					break;
				case DictValue.Types.Vector2:
					myTarget.startValues.Insert (0, new DictValue(new Vector2(0,0)));
					break;
				case DictValue.Types.Vector3:
					myTarget.startValues.Insert (0, new DictValue(new Vector3(0,0,0)));
					break;
				case DictValue.Types.GameObject:
					GameObject ob = new GameObject();
					myTarget.startValues.Insert (0, new DictValue(ob));
					DestroyImmediate(ob);
					break;
				}
				int suffix = 0;
				string newKey;
				do {
					suffix++;
					newKey = newType.ToString()+"_"+suffix.ToString();
				} while (myTarget.startKeys.Contains(newKey));
				myTarget.startKeys.Insert (0, newKey);
			}

			EditorGUILayout.EndHorizontal();
			for (int i=0; i < myTarget.startKeys.Count; i++) {
				EditorGUILayout.BeginHorizontal();
				string newKey = EditorGUILayout.TextField(myTarget.startKeys[i], GUILayout.MinWidth(50));
				if ( newKey == "" || (newKey != myTarget.startKeys[i] && myTarget.startKeys.Contains(newKey))){
					myTarget.startKeys[i] = "#ERROR#";
				}
				else {
					myTarget.startKeys[i] = newKey;
				}
				switch (myTarget.startValues[i].dataType) {
				case DictValue.Types.Float:
					myTarget.startValues[i].floatValue = 
						EditorGUILayout.FloatField(myTarget.startValues[i].floatValue, GUILayout.MinWidth(50));
					break;
				case DictValue.Types.Int:
					myTarget.startValues[i].intValue = 
						EditorGUILayout.IntField(myTarget.startValues[i].intValue, GUILayout.MinWidth(50));
					break;
				case DictValue.Types.String:
					myTarget.startValues[i].stringValue = 
						EditorGUILayout.TextField(myTarget.startValues[i].stringValue, GUILayout.MinWidth(50));
					break;
				case DictValue.Types.Vector2:
					myTarget.startValues[i].vector2Value = 
						EditorGUILayout.Vector2Field("",myTarget.startValues[i].vector2Value, GUILayout.MinWidth(50));
					break;
				case DictValue.Types.Vector3:
					myTarget.startValues[i].vector3Value = 
						EditorGUILayout.Vector3Field("",myTarget.startValues[i].vector3Value, GUILayout.MinWidth(50));
					break;
				case DictValue.Types.GameObject:
					myTarget.startValues[i].objectValue = 
							(GameObject)EditorGUILayout.ObjectField(myTarget.startValues[i].objectValue,
					                                     typeof(GameObject),
					                                     true, GUILayout.MinWidth(50));
					break;
				}
				if (GUILayout.Button(new GUIContent("X","Delete this entry"), 
				                     new GUILayoutOption[] { GUILayout.Width (buttonWidth), 
															GUILayout.Height(15)} )) {
					myTarget.startKeys.RemoveAt(i);
					myTarget.startValues.RemoveAt(i);
				}
				if (GUILayout.Button(new GUIContent("V","Move down"), 
				                     new GUILayoutOption[] { GUILayout.Width (buttonWidth), 
															GUILayout.Height(15)} )) {
					if (i < myTarget.startKeys.Count-1) {
						GUIUtility.keyboardControl = 0;
						GUIUtility.hotControl = 0;
						string k = myTarget.startKeys[i];
						DictValue v = myTarget.startValues[i];
						myTarget.startKeys.RemoveAt(i);
						myTarget.startValues.RemoveAt(i);
						myTarget.startKeys.Insert (i+1, k);
						myTarget.startValues.Insert (i+1, v);
						EditorUtility.SetDirty(myTarget);
					}
				}
				EditorGUILayout.EndHorizontal();
			}

		}

	}


}
