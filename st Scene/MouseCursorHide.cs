using UnityEngine;
using System.Collections;

[AddComponentMenu("st Scene/Mouse Cursor Hide")]

public class MouseCursorHide : MonoBehaviour {
	// make the mouse cursor invisible
	// if you put this on st Main, the mouse will be hidden in all your scenes!
	void Start () {
		Screen.showCursor = false;
	}
}
