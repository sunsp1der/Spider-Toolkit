using UnityEngine;
using System.Collections;
using System.IO;

[AddComponentMenu("st Object/Input/Button Scene")]

public class ButtonScene : ButtonCallback {

	[Tooltip("Name of scene to load next. * = reload current scene.")]
	public string nextScene = "*";

	public override void OnClick (){
		if (nextScene != "") {
			stTools.EndScene(nextScene);
		}
		else if (nextScene == "*"){
			stTools.EndScene( Application.loadedLevelName);
		}
	}
}
