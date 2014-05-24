using UnityEngine;
using System.Collections;

public class stTools {
	// various utility functions

	static GameObject _stmain;

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

	public static void LoadScene (string scene) {
		// use this instead of Application.loadlevel to keep 'st Main' persistent between levels
		GameObject stmain = GameObject.FindGameObjectWithTag("st Main");
		stmain.GetComponent<stMain>().undying = true;
		stmain.transform.parent = null;
		Application.LoadLevel (scene);
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
}




