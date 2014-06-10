using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Audio/Mouse Click Sound")]


public class MouseClickSound : MonoBehaviour {
	// Play Sound when object is clicked. 
	// Object must have an activated collider

	public Sound sound;
	[Tooltip("Play when clicked and released.")]
	public bool asButton = false;
	[Tooltip("Stop sound when mouse released")]
	public bool stopOnMouseUp = false; 

	void OnMouseDown() {
		if (!asButton) sound.Play();
	}

	void OnMouseUpAsButton() {
		if (asButton) sound.Play();
	}

	void OnMouseUp() {
		if (stopOnMouseUp) {
			sound.Stop();
		}
	}
}
