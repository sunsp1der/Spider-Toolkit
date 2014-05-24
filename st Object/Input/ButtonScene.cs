using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Input/Button Scene")]
[RequireComponent(typeof(SpriteRenderer))]

public class ButtonScene : ButtonCallback {

	public string scene;
	public float sceneDelay = 0; // number of seconds before changing levels

	void OnClick (){
		if (scene != "") {
			Invoke("DoScene", sceneDelay);
		}
	}

	void DoScene() {
		stTools.LoadScene(scene);
	}
}
