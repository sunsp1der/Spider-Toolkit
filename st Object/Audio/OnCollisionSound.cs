using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Audio/On Collision Sound")]

public class OnCollisionSound : MonoBehaviour {

	public Sound sound;

	void OnCollisionEnter2D (Collision2D info) {
		sound.Play();
	}
}
