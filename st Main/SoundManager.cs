using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(stMain))]

public class SoundManager : MonoBehaviour {

	[System.Serializable]
	public class AudioInfo {
		public float startTime;
		public bool locked = false; // keeps sound from being replaced
		public AudioSource source;
	}
	public AudioInfo[] audioInfo;
	
	void Awake () {
		stMain main = stTools.stMain;
		List<AudioSource> audioSources = GetComponents<AudioSource>().ToList();
		// add any new audio sources necessary
		for (int i = audioSources.Count; i < main.audioSources; i++) {
			audioSources.Add ( main.gameObject.AddComponent<AudioSource>());
		}
		// add all AudioSources to audioInfo array
		audioInfo = new AudioInfo[audioSources.Count];
		for ( int i = 0; i<audioSources.Count; i++) {
			audioInfo[i] = new AudioInfo();
			audioInfo[i].source = audioSources[i];
			audioSources[i].clip = null;
		}
	}

	public AudioSource Play ( Sound sound, bool locked=false) {
		if (Application.isPlaying) {
			int earliestSound = -1;
			float earliestTime = Time.time;
			int i;
			for ( i=0; i<audioInfo.Length; i++) {
				if (!audioInfo[i].source.loop && !audioInfo[i].locked) {
					if (audioInfo[i].source.isPlaying) {
						if (audioInfo[i].startTime < earliestTime) {
							earliestSound = i;
							earliestTime = audioInfo[i].startTime;
						}
					}
					else break;
				}
			}
			// if an empty sound is not found, play the earliest started one 
			// that is not looping or locked. If none, don't play sound
			if (i>=audioInfo.Length) {
				if (earliestSound != -1) {
					i = earliestSound;
				}
				else {
					return null;
				}
			}
			// set up sound
			audioInfo[i].startTime = Time.time;
			audioInfo[i].locked = locked;
			audioInfo[i].source.clip = sound.audioClip;
			audioInfo[i].source.volume = sound.volume;
			audioInfo[i].source.loop = sound.loop;
			audioInfo[i].source.pan = -sound.pan;
			audioInfo[i].source.pitch = sound.pitch;
			audioInfo[i].source.Play();
			return audioInfo[i].source;
		}
		else {
			AudioSource source = GetComponent<AudioSource>();
			source.Stop ();
			source.clip = sound.audioClip;
			source.volume = sound.volume;
			source.loop = sound.loop;
			source.pan = -sound.pan;
			source.pitch = sound.pitch;
			source.Play(); 
			return source;
		}
	}
}