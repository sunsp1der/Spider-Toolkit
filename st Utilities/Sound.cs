using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class Sound {

	public AudioClip audioClip;
	public bool loop = false; // loop the sample
	[Range(0,1)]
	public float volume = 1.0f;
	[Range(-1,1)]
	public float pan = 0; // pan left to right
	public bool locked;
	[Range(0.01f,3)]
	public float pitch = 1; // pitch shift

	static SoundManager soundManager;
	public AudioSource mySource = null;
	public class FadeData {
		public float fadeAmount;
		public float from;
		public float seconds;
		public float currentFade;
		public float elapsed;
		public bool stopWhenDone;
	}
	public FadeData fadeData = new FadeData();
#if UNITY_EDITOR
	public bool foldout = false;
#endif

	public AudioSource Play( float fadein = 0){
		// fadein is # of seconds to take fading in the sound
		if ( audioClip == null) return mySource;
		if (soundManager == null) {
			soundManager = GameObject.FindGameObjectWithTag("st Main").GetComponent<SoundManager>();
			if (soundManager == null) {
				Debug.LogError("No 'st Main' object... add 'st Main' prefab as a child of main camera.");
			}
		}
		if (audioClip == null) return null;
		mySource = soundManager.Play( this);

		if (fadein > 0) {
			FadeIn(fadein);
		}
		return mySource;
	}
	
	public void FadeIn(float seconds, float fadeTo=-1)
	{
		fadeData.stopWhenDone = false;
		// fadeTo is volume to fade to. If < 0, fade to volume
		if (fadeTo < 0) {
			
			fadeTo = volume;
		}
		FadeTo (seconds, 0, fadeTo);
	}

	public void FadeOut(float seconds, bool stopWhenDone=true)
	{
		if (mySource == null) return;
		FadeTo (seconds, mySource.volume, 0, stopWhenDone);
	}

	public void FadeTo(float seconds, float from, float to, bool stopWhenDone=false) {
		// from is volume to fade from, to is volume to fade to
		if (seconds <= 0 || from == to) {
			return;
		}
		fadeData.stopWhenDone = stopWhenDone;
		fadeData.from = from;
		fadeData.seconds = seconds;
		fadeData.elapsed = 0;
		fadeData.fadeAmount = to - from;
		if (soundManager == null) {
			soundManager = GameObject.FindGameObjectWithTag("st Main").GetComponent<SoundManager>();
			if (soundManager == null) {
				Debug.LogError("No 'st Main' object... add 'st Main' prefab as a child of main camera.");
			}
		}
		mySource.volume = from;
		soundManager.StartCoroutine (DoFade());
	}

	public IEnumerator DoFade() {
		while (true) {
			fadeData.currentFade = fadeData.fadeAmount * (fadeData.elapsed)/fadeData.seconds;
			if ( (fadeData.fadeAmount > 0 && fadeData.currentFade > fadeData.fadeAmount) || 
			           (fadeData.fadeAmount < 0 && fadeData.currentFade < fadeData.fadeAmount)) {
				break;
			}
			mySource.volume = fadeData.from + fadeData.currentFade;
			yield return new WaitForSeconds(0.1f);
			fadeData.elapsed += 0.1f;
		}
		mySource.volume = fadeData.from + fadeData.fadeAmount;
		if (fadeData.stopWhenDone) {
			mySource.Stop();
		}
	}

	public void Stop() {
		mySource.Stop();
	}

}
