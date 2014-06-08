using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(stMain))]
public class stMainEditor : Editor {

	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		stEditor.InspectorLine();
		GUILayout.BeginHorizontal();
		GUILayout.Space(35);
		GUILayout.Label("Editor Options",  EditorStyles.boldLabel);
		GUILayout.EndHorizontal();
		bool oldAutoSave = PlayerPrefs.GetInt("AutoSave") != 0;
		bool autoSave;
		autoSave = GUILayout.Toggle( oldAutoSave, new GUIContent ("Auto Save scene when run", "Auto save the scene every time you press play."));
		if (autoSave != oldAutoSave) {
			PlayerPrefs.SetInt("AutoSave",autoSave?-1:0);
		}
	}

	void OnEnable(){
		Tools.current = Tool.None;
	}
}
