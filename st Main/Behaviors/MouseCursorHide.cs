using UnityEngine;
using System.Collections;

[AddComponentMenu("st Main/Behaviors/Mouse Cursor Hide")]

public class MouseCursorHide : MonoBehaviour {
	// make the mouse cursor invisible
	void Start () {
		Screen.showCursor = false;
	}
}
