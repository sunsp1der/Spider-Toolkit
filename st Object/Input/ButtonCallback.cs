using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Input/Button Callback")]
[RequireComponent(typeof(SpriteRenderer))]


public class ButtonCallback : MonoBehaviour {
	/*
	 * Generic button behavior. Base class for other buttons
	 * onClickCallback sends OnClick to other components on this object
	 * 
	 * Needs an active collider to work!
	 */

	[Tooltip("Tint when hovering over object")]
	public Color hoverColor = new Color (199.0f/255, 242.0f/255, 228.0f/255); 
	[Tooltip("Tint while pressing mouse down on object")]
	public Color pressColor = new Color(104.0f/255, 145.0f/255, 212.0f/255);
	[Tooltip("Sound when mouse enters button area")]
	public Sound enterSound; // plays when mouse goes over button
	[Tooltip("Sound when button is clicked")]
	public Sound clickSound; // plays when button is clicked
	[Tooltip("Call OnClick on all components when button is clicked")]
	public bool onClickCallback = true;

	Color baseColor = new Color(1, 1, 1);
	SpriteRenderer sprite;

	void Start () {
		sprite = GetComponent<SpriteRenderer>();
		baseColor = sprite.color;
	}
	
	void OnMouseUpAsButton(){
		if (clickSound != null) {
			clickSound.Play();
		}
		if (onClickCallback) {
			SendMessage("OnClick");
		}
		else {
			OnClick();
		}
	}

	public virtual void OnClick() {

	}

	void OnMouseEnter () {
		if (enterSound != null) {
			enterSound.Play();
		}
		try { // was getting intermittent sprite = null error. Unity bug?
				sprite.color = hoverColor;
		}
		catch{}
	}

	void OnMouseExit() {
		sprite.color = baseColor;
	}
	
	void OnMouseDown(){
		sprite.color = pressColor;
	}

	void OnMouseUp(){
		sprite.color = hoverColor;
	}
}
