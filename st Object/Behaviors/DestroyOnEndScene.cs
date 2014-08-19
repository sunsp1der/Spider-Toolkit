using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Behaviors/Destroy On EndScene")]

public class DestroyOnEndScene : MonoBehaviour {
	// Destroy this object when EndScene is called
	// Useful for delayed EndScenes

	public bool doRemoveEffects;

	void OnEndScene(string sceneName) {
		if (doRemoveEffects) {
			stTools.Remove(gameObject);
		}
		else {
			Destroy( gameObject);
		}
	}
}
