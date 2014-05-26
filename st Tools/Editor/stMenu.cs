using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class stMenu : EditorWindow {			

	[MenuItem( "Tools/Spider's Toolkit/Create/Object", false, 0 )]	
	public static void NewObject() {
		GameObject noob = (GameObject)Instantiate(Resources.LoadAssetAtPath("Assets/Spider-Toolkit/st Object/st Object.prefab", typeof(GameObject)));
		noob.name = "Object";
	}

	[MenuItem( "Tools/Spider's Toolkit/Create/Text", false, 0 )]	
	public static void NewText() {
		GameObject noob = (GameObject)Instantiate(Resources.LoadAssetAtPath("Assets/Spider-Toolkit/st Text/st Text.prefab", typeof(GameObject)));
		noob.name = "Text";
	}

	[MenuItem( "Tools/Spider's Toolkit/Create/New Scene", false, 0 )]	
	public static void NewScene() {
		EditorApplication.SaveCurrentSceneIfUserWantsTo();
		EditorApplication.NewScene();
		GameObject stmain = GameObject.Instantiate( Resources.LoadAssetAtPath("Assets/Spider-Toolkit/st Main/st Main.prefab", typeof(GameObject)) ) as GameObject;
		stmain.name = "st Main";
		stmain.transform.parent = Camera.main.transform	;
		Camera.main.backgroundColor = Color.black;
	}

	[MenuItem( "Tools/Spider's Toolkit/Set Up ST Default Collision Layers", false, 0 )]	
	public static void AddSTLayers() {
		SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
		
		SerializedProperty it = tagManager.GetIterator();
		bool showChildren = true;
		Dictionary<string,string> layers = new Dictionary<string, string>()
		{
				{"User Layer 28","Player"},
				{"User Layer 29","Player Shot"},
				{"User Layer 30","Enemy"},
				{"User Layer 31","Enemy Shot"},
		};
		bool errors = false;
		while (it.NextVisible(showChildren))
		{
			//set your tags here
			if ( layers.ContainsKey(it.name))
			{
				if (it.stringValue == "") {
					it.stringValue = layers[it.name];
				}
				else if (it.stringValue != layers[it.name]) {
					Debug.LogError("User Layer 31 already being used. Unable to set up ST default layers!");
					errors = true;
					break;
				}
			}
		}
		if (errors) {
			Debug.LogError ("Collision Masks not set up due to error creating layers.");
		}
		else {
			Physics2D.IgnoreLayerCollision(28, 28, true);
			Physics2D.IgnoreLayerCollision(28, 29, true);
			Physics2D.IgnoreLayerCollision(28, 30, false);
			Physics2D.IgnoreLayerCollision(28, 31, false);
			Physics2D.IgnoreLayerCollision(29, 29, true);
			Physics2D.IgnoreLayerCollision(29, 30, false);
			Physics2D.IgnoreLayerCollision(29, 31, true);
			Physics2D.IgnoreLayerCollision(30, 30, true);
			Physics2D.IgnoreLayerCollision(30, 31, true);
			Physics2D.IgnoreLayerCollision(31, 31, true);
		}
		tagManager.ApplyModifiedProperties();
	}

	[MenuItem( "CONTEXT/Component/Get Component" )]
	public static void GetComponent( MenuCommand command )
	{
		stEditor.clipboard = command.context as Component;
		Debug.Log ("Got " + command.context.ToString());
	}	


}