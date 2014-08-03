using UnityEngine;
using System.Collections;

public static class stTools {
	// various utility functions

	static stMain _stmain;
	public static string pendingScene = "";
	public static float endSceneAtTime = 0;

	/// <summary>
	/// Easy access to the st Main object (not the component on that object!)
	/// </summary>
	/// <value>The st main.</value>
	public static stMain stMain{
		get {
			if (_stmain == null) {
				_stmain = GameObject.FindGameObjectWithTag("st Main").GetComponent<stMain>();
			}
			return _stmain;
		}
	}

	#region miscellaneous utility functions

	/// <summary>
	/// Makes a unique name for a gameobject. If any other objects have the name nameRoot,
	/// this will add a unique number to the end.
	/// </summary>
	/// <returns>The unique name.</returns>
	/// <param name="nameRoot">The un-numbered name</param>
	/// <param name="forceNumber">If no object called nameRoot exists, add the number 1;
	public static string MakeUniqueObjectName (string nameRoot, bool forceNumber=false){
		string name = nameRoot;
		int num = 1;
		if (forceNumber) {
			name += " 1";
		}
		while (GameObject.Find(name)) {
			num++;
			name = nameRoot + " " + num.ToString();
		}
		return name;
	}

	/// <summary>
	/// Gets the input axis as a 1, 0, or -1 (no auto softening).
	/// </summary>
	/// <returns>The hardened axis.</returns>
	/// <param name="input">Input axis from settings/input.</param>
	public static float GetAxisBool (string input){
		if (!Input.GetButton(input)) {
			return 0;
		}
		else {
			float i = Input.GetAxis(input);
			return i > 0 ? 1 : -1;
		}
	}

	/// <summary>
	/// call a function on all gameobjects
	/// </summary>
	/// <param name="fn">The function to call</param>
	public static void BroadcastAll(string fn) {
		GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject go in gos) {
			if (go && go.transform.parent == null) {
				go.gameObject.BroadcastMessage(fn, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	/// <summary>
	/// call a function on all gameobjects with an argument
	/// </summary>
	/// <param name="fn">The function</param>
	/// <param name="arg">The argument</param>
	public static void BroadcastAll(string fn, object arg) {
		GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject go in gos) {
			if (go && go.transform.parent == null) {
				go.gameObject.BroadcastMessage(fn, arg, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	/// <summary>
	/// remove an object using st remove behaviors
	/// works on non st objects as normal destroy
	/// </summary>
	/// <param name="obj">Object.</param>
	public static void Remove( GameObject obj) {
		stObject stobject = obj.GetComponent<stObject>();
		if (stobject != null) {
			stobject.Remove();
		}
		else {
			Object.Destroy (obj);
		}
	}
	#endregion
	
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

	#region endscene
	/// <summary>
	/// Spider Toolkit alternative to Application.loadlevel. 
	/// Ends a scene after any delays (blackouts etc.) then load a new level.  
	/// Also keeps 'st Main' persistent between levels.
	/// </summary>
	/// <param name="scene">The new scene.</param>
	public static void EndScene (string nextScene) {

		pendingScene = nextScene;
		BroadcastAll ("OnEndScene", nextScene);
		// call in stMain's help because it's a monobehavior instance and we can do invokes there
		stMain.CheckEndScene();
	}

	/// <summary>
	/// Switch to pending new scene immediately ignoring delays. Not generally meant to be called by user.
	/// </summary>
	public static void DoEndScene () {
		if (pendingScene != "") {
			string scene = pendingScene;
			pendingScene = "";
			endSceneAtTime = 0;
			stMain.undying = true;
			stMain.gameObject.transform.parent = null;
			Application.LoadLevel (scene);
		}
	}

	/// <summary>
	/// Call to delay the switch to the pending new scene
	/// </summary>
	/// <param name="delaySecs">Delay secs.</param>
	public static void DelayEndScene( float delaySecs) {
		// delay: seconds to wait to destroy
		if (delaySecs <= 0) return;
		float dTime = delaySecs + Time.time;
		if (dTime > endSceneAtTime) {
			endSceneAtTime = dTime;
		}
	}
	
	#endregion
}




