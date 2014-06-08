using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Behaviors/Delay NewScene")]

public class DelayNewScene : MonoBehaviour {
	// This object delays NewScene calls
	public float delaySecs = 3; // seconds before object self destructs

	void OnNewScene(string sceneName) {
		stTools.DelayNewScene( delaySecs);
	}
}
