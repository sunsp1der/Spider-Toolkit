using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Audio/Mouse Click Sound")]


public class MouseClickSound : MonoBehaviour {
	// Play Sound when object is clicked. 
	// Object must have an activated collider

	public Sound sound;
	public bool click = false; // if false, play when pressed and released
							  // on object. like a button

	void OnMouseDown() {
		if (click) sound.Play();
	}

	void OnMouseUpAsButton() {
		if (!click) sound.Play();
	}
}
