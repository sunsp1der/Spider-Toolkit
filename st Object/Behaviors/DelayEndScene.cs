// This object delays stTools.EndScene calls


using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Behaviors/Delay EndScene")]

public class DelayEndScene : MonoBehaviour {
	public float delaySecs = 3; // seconds before object self destructs

	void OnEndScene(string sceneName) {
		stTools.DelayEndScene( delaySecs);
	}
}
