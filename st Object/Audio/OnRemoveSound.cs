using UnityEngine;
using System.Collections;
[AddComponentMenu("st Object/Audio/On Remove Sound")]

public class OnRemoveSound : MonoBehaviour {
	// Remove is used by stObjects to allow special behaviors at the end of an objects life, such as fadeout

	public Sound sound;

	void OnRemove()
	{
		sound.Play();
	}
}
