using UnityEngine;
using System.Collections;

[AddComponentMenu("st Main/Behaviors/Mouse Cursor Hide")]
/// <summary>
/// Mouse cursor hide. Make the mouse cursor invisible.
/// </summary>
public class MouseCursorHide : MonoBehaviour {
	void Start () {
		Screen.showCursor = false;
	}

}