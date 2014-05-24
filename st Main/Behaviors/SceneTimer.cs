using UnityEngine;
using System.Collections;

[AddComponentMenu("st Main/Behaviors/SceneTimer")]

public class SceneTimer : MonoBehaviour {
	// Change to new scene when timer runs out

	public float seconds = 60; // timer length
	public string newScene = ""; // scene to change to, if blank, no scene change
	public string dictionary; // if this and key are not empty, store time left in this dictionary
	public string key; // if this and dictionary are not empty, store time elapsed in this key
	[HideInInspector]
	public float timeLeft;
	[HideInInspector]
	public float timeElapsed;
	public LockedView _timeLeft;
	public LockedView _timeElapsed;

	[HideInInspector]
	public float startTime;
	bool timerDone;
	stDictionary dict = null;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		if (dictionary != "" && key != ""){
			dict = stData.InitializeDictionaryValue(dictionary, key, seconds);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (timerDone) return;
		timeElapsed = Time.time - startTime;
		timeLeft = seconds - timeElapsed;
		if (timeLeft < 0) {
			timeLeft = 0;
			timeElapsed = seconds;
			if (newScene != "") {
				stTools.LoadScene(newScene);
				timerDone = true;
			}
		}
		if (dict) {
			dict.Set (key,timeLeft);
		}
	}
}
