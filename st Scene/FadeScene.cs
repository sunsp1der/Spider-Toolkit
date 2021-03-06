﻿using UnityEngine;
using System.Collections;

[AddComponentMenu("st Scene/FadeScene")]

public class FadeScene : MonoBehaviour {
	// If this is on an object in the scene, fade the whole scene in and/or out
	// May not work with particle effects!!!
	// Generally works well to attach this to camera object.
	[Tooltip("0 = no fade")]
	public float fadeInSecs = 2; 
	[Tooltip("0 = no fade")]
	public float fadeOutSecs = 2;
	[Tooltip("After faded out, wait this long extra before loading new scene")]
	public float blackedOutSecs = 0;
	[Tooltip("The object to put in front of camera for fade. Different effects can be created by making your own.")]
	public GameObject fadeCover; 

	GameObject fadeCoverInstance; // this will store the object we put in front of the camera

	// This runs when component is added. Sets up default fadeCover.
	void Reset() {
		if (fadeCover == null) {
			fadeCover = (GameObject)Resources.LoadAssetAtPath("Assets/Spider-Toolkit/st Scene/FadeSceneObject.prefab", typeof(GameObject));
		}
	}

	void Start() {
		if (fadeInSecs != 0 && fadeCover) {
			fadeCoverInstance =  (GameObject) Instantiate (fadeCover);
			fadeCoverInstance.transform.parent = Camera.main.transform;
			fadeCoverInstance.transform.localPosition =  new Vector3(0,0,1);
			Fade fader = fadeCoverInstance.GetComponent<Fade>();
			fader.fadeInSeconds = 0;
			fader.fadeOutSeconds = fadeInSecs; // we want our blackness to fade out so our scene fades in!
			stTools.Remove( fadeCoverInstance);
		}
	}

	void OnEndScene(string sceneName) {
		stTools.DelayEndScene( fadeOutSecs + blackedOutSecs);
		fadeCoverInstance = (GameObject) Instantiate(fadeCover);
		Fade fader = fadeCoverInstance.GetComponent<Fade>();
		fader.fadeInSeconds = fadeOutSecs; // we want our blackness to fade in so our scene fades out!
		fader.fadeOutSeconds = 0; 
	}
}
