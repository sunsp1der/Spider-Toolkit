using UnityEngine;
using System.Collections;
[AddComponentMenu("st Object/Audio/Key Sound")]

public class KeySound : MonoBehaviour {

	public Sound sound;
	public string inputName = "Jump"; // see project settings/input
	public bool stopOnKeyUp = false; // loop sound while key is down

	bool playing = false;

	void Update () {
		if (Input.GetButtonDown(inputName) && !playing) {
			sound.Play();
			playing = true;
		}
		else if (stopOnKeyUp && Input.GetButtonUp(inputName)){
			sound.Stop();
			playing = false;
		}
	}
}

