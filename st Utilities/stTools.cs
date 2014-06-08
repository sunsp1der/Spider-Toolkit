using UnityEngine;
using System.Collections;

public class stTools {
	// various utility functions

	static GameObject _stmain;
	public static string pendingScene = "";
	public static float newSceneAtTime = 0;

	public static void BroadcastAll(string fun) {
		GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject go in gos) {
			if (go && go.transform.parent == null) {
				go.gameObject.BroadcastMessage(fun, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public static void BroadcastAll(string fun, object msg) {
		GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject go in gos) {
			if (go && go.transform.parent == null) {
				go.gameObject.BroadcastMessage(fun, msg, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	public static void Remove( GameObject obj) {
		// remove an object using st remove behaviors
		// works on non st objects as normal destroy
		stObject stobject = obj.GetComponent<stObject>();
		if (stobject != null) {
			stobject.Remove();
		}
		else {
			Object.Destroy (obj);
		}
	}
	
	public static GameObject stMain{
		get {
			if (_stmain == null) {
				_stmain = GameObject.FindGameObjectWithTag("st Main");
			}
			return _stmain;
		}
	}

	#region stTools.Spawn
	// Use stTools.Spawn instead of instantiate for stObjects.
	// You MUST use this when instantiating an stObject.archetype
	public static GameObject Spawn( GameObject original) {
		// special instantiate that works with archetypes

		GameObject newObject = (GameObject) Object.Instantiate( original);
		checkArchetype (newObject, original);
		return newObject;
	}
	// st Spawn with position. 
	public static GameObject Spawn(  GameObject original, Vector3 position, Quaternion rotation) {
		// special instantiate that works with archetypes
		
		GameObject newObject = (GameObject) Object.Instantiate( original, position, rotation);
		checkArchetype (newObject, original);
		return newObject;
	}
	// allow spawning of archetype objects
	static void checkArchetype( GameObject newObject, GameObject original) {
		// special archetype component for spawnable scene objects
		stObject stobject = newObject.GetComponent<stObject>();
		if (stobject != null && stobject.myArchetype != null) {
			newObject.SetActive (true);
		}	
	}
	#endregion

	#region newscene
	public static void NewScene (string scene) {
		// use this instead of Application.loadlevel to keep 'st Main' persistent between levels
		pendingScene = scene;
		BroadcastAll ("OnNewScene", scene);
		// call in stMain's help because it's a monobehavior instance and we can do invokes there
		stMain.GetComponent<stMain>().CheckNewScene();
	}

	public static void DoNewScene () {
		if (pendingScene != "") {
			string scene = pendingScene;
			pendingScene = "";
			newSceneAtTime = 0;
			stMain.GetComponent<stMain>().undying = true;
			stMain.transform.parent = null;
			Application.LoadLevel (scene);
		}
	}
	
	public static void DelayNewScene( float delaySecs) {
		// delay: seconds to wait to destroy
		if (delaySecs <= 0) return;
		float dTime = delaySecs + Time.time;
		if (dTime > newSceneAtTime) {
			newSceneAtTime = dTime;
		}
	}
	
	#endregion
}




