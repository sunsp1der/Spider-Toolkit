using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class stMenu : EditorWindow {			

	static GameObject CreateObject( string pathToAsset, string baseName) {
		// instantiate new object from resources
		GameObject newObject = (GameObject)Instantiate(Resources.LoadAssetAtPath( pathToAsset, typeof(GameObject)));
		// give it a unique name
		newObject.name = stTools.MakeUniqueObjectName(baseName, true);
		// give it a unique location
		GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;
		bool duplicateLocation;
		do {
			duplicateLocation = false;
			foreach(GameObject go in allObjects) {
				if (go != newObject && go.activeInHierarchy && 
				    						go.transform.position == newObject.transform.position) {
					newObject.transform.position += new Vector3 (0.3f, -0.3f, 0);
					duplicateLocation = true;
				}
			}
		} while (duplicateLocation);
		// select it
		Selection.activeObject = newObject;
		Undo.RegisterCreatedObjectUndo( newObject, "Create "+baseName);
		return newObject;
	}

	[MenuItem( "Spider-Toolkit/Create Object", false, 0 )]	
	public static void NewObject() {
		CreateObject ("Assets/Spider-Toolkit/st Object/st Object.prefab", "Object");
	}

	[MenuItem( "Spider-Toolkit/Create Text", false, 0 )]	
	public static void NewText() {
		CreateObject ("Assets/Spider-Toolkit/st Text/st Text.prefab", "Text");
	}

	[MenuItem( "Spider-Toolkit/Create Button", false, 0 )]	
	public static void NewButton() {
		CreateObject ("Assets/Spider-Toolkit/st Object/st Button.prefab", "Button");
	}

	[MenuItem( "Spider-Toolkit/New Scene", false, 0 )]	
	public static void EndScene() {
		EditorApplication.SaveCurrentSceneIfUserWantsTo();
		EditorApplication.NewScene();
		GameObject stmain = (GameObject) GameObject.Instantiate( 
						Resources.LoadAssetAtPath("Assets/Spider-Toolkit/st Main/st Main.prefab", typeof(GameObject)));
		stmain.name = "st Main";
		stmain.transform.parent = Camera.main.transform;
		Camera.main.backgroundColor = Color.black;
		Camera.main.orthographic = true;
	}

	[MenuItem( "Spider-Toolkit/Tools/Set ST Default Collision Layers", false, 0 )]	
	public static void AddSTLayers() {
		SerializedObject tagManager = 
					new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
		
		SerializedProperty it = tagManager.GetIterator();
		bool showChildren = true;
		Dictionary<string,string> layers = new Dictionary<string, string>()
		{
			{"User Layer 26","Collide All"},
			{"User Layer 27","No Collide"},
			{"User Layer 28","Player"},
			{"User Layer 29","Player Shot"},
			{"User Layer 30","Target"},
			{"User Layer 31","Target Shot"},
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
					Debug.LogError("User Layer '"+it.stringValue+"' already assigned. Delete it and try again.");
					errors = true;
					break;
				}
			}
		}
		if (errors) {
			Debug.LogError ("Collision Masks not set up due to error creating layers.");
		}
		else {
			for (int i = 0; i<=31; i++) {
				Physics2D.IgnoreLayerCollision(26, i, false);
			}
			for (int i = 0; i<=31; i++) {
				Physics2D.IgnoreLayerCollision(27, i, true);
			}
			Physics2D.IgnoreLayerCollision(27, 28, true);
			Physics2D.IgnoreLayerCollision(27, 29, true);
			Physics2D.IgnoreLayerCollision(27, 30, true);
			Physics2D.IgnoreLayerCollision(27, 31, true);
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