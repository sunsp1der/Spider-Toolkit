// When this AND ALL OTHER OBJECTS WITH THIS COMPONENT are removed from the scene, start a new scene

using UnityEngine;
using System.Collections;

public class EndSceneOnRemove : MonoBehaviour {

	[Tooltip("The next level to load")]
	public string nextLevel;
	[Tooltip("End level when this object is removed, ignoring others with this component")]
	public bool ignoreOthers = false;

	static int numObjects = 0; // number of objects in scene with this component

	// Use this for initialization
	void Start () {
		numObjects++;
	}
	
	void OnRemove() {
		numObjects--;
		if (numObjects == 0 || ignoreOthers) {
			stTools.EndScene(nextLevel);
		}
	}

	void OnNewScene() {
		numObjects = 0;
	}
}
