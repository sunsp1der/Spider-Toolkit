using UnityEngine;
using System.Collections;
[AddComponentMenu("st Object/Audio/On Destroy Sound")]

public class OnDestroySound : MonoBehaviour {

	public Sound sound;

	bool isQuitting = false;

	void OnApplicationQuit()
	{
	    isQuitting = true;
	}

	void OnDestroy()
	{
	    if (!isQuitting)
	    {
			sound.Play();
	    }
	}
}
