using UnityEngine;
using UnityEditor;

using System;
using System.Linq;

// various utility functions. Use this class as a singleton

public class stEditor
{	
	public static Component clipboard;

	// untested
	public static Rect PropertyDrawerLine( Rect position, float height=2){
		Color savedColor = GUI.color;
		float savedHeight = position.height;
		position.height = 2;
		GUI.color = Color.black;
		GUI.Box (position, "");
		GUI.color = savedColor;	
		position.y += 2;
		position.height = savedHeight;
		return position;
	}

	public static void InspectorLine( int height=2, int padding=2 ) {
		GUILayout.Space( padding );
		Color savedColor = GUI.color;
		GUI.color = Color.black;	
		GUILayout.Box( "", GUILayout.ExpandWidth( true ), GUILayout.Height( height ) );
		GUI.color = savedColor;
		GUILayout.Space( padding );
	}

	public static void RegisterUndo( UnityEngine.Object target, string UndoMessage )
	{
		#if UNITY_4_3
		Undo.RecordObject( target, UndoMessage );
		#else
		Undo.RegisterSceneUndo( UndoMessage );
		#endif
		EditorUtility.SetDirty( target );		
	}
	
}

	