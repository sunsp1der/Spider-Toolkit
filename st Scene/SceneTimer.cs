using UnityEngine;
using System.Collections;

[AddComponentMenu("st Scene/SceneTimer")]

public class SceneTimer : MonoBehaviour {
	// Change to new scene when timer runs out
	// if you put this on st Main, there will be a timer in all your scenes!


	public float seconds = 60; // timer length
	public string endScene = ""; // scene to change to, if blank, no scene change
	public string dictionary = ""; // if provided store timeleft in dict and key
	public string key = ""; // if provided store timeleft in dict and key
	[HideInInspector]
	public float timeLeft;
	public LockedView _timeLeft;
	[HideInInspector]
	public float timeElapsed;
	public LockedView _timeElapsed;
	stDictionary dict = null;

	[HideInInspector]
	public float startTime;
	bool timerDone;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		if (dictionary != "" && key != "") {
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
			if (endScene != "") {
				stTools.EndScene(endScene);
				timerDone = true;
			}
		}
		if (dict != null) {
			dict.Set (key, timeLeft);
		}
	}
}
