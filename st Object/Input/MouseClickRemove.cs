using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Input/Mouse Click Remove")]


public class MouseClickRemove : MonoBehaviour {
	// Destroy when object is clicked. 
	// Object must have an activated collider

	public bool click = true; // if false, destroy when pressed and released
							  // on object. like a button

	void OnMouseDown() {
		if (click) stTools.Remove(gameObject);
	}

	void OnMouseUpAsButton() {
		if (!click) stTools.Remove(gameObject);
	}
}
