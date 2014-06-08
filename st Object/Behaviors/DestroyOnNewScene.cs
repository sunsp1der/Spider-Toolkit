using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Behaviors/Destroy On NewScene")]

public class DestroyOnNewScene : MonoBehaviour {
	// Destroy this object when NewScene is called
	// Useful for delayed NewScenes
	public bool doRemoveEffects;

	void OnNewScene(string sceneName) {
		if (doRemoveEffects) {
			stTools.Remove(gameObject);
		}
		else {
			Destroy( gameObject);
		}
	}
}
