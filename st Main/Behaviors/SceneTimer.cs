using UnityEngine;
using System.Collections;

[AddComponentMenu("st Main/Behaviors/SceneTimer")]

public class SceneTimer : MonoBehaviour {
	// Change to new scene when timer runs out

	public float seconds = 60; // timer length
	public string newScene = ""; // scene to change to, if blank, no scene change
	public float timeLeft;
	public float timeElapsed;

	[HideInInspector]
	public float startTime;
	bool timerDone;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
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
	}
}
