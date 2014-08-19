using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Behaviors/Self Destruct")]


public class SelfDestruct : MonoBehaviour {
	// Remove object after given amount of time
	[Tooltip("Seconds before object self destructs")]
	public float timerSecs = 3; 
	[Tooltip("TimerSecs can vary this much")]
	public float timerSecsVariance = 0;
	[Tooltip("Start timer when object is created")]
	public bool start_automatically = true; // otherwise call 'StartTimer' method
	public bool doRemoveEffects = true;
	
	void Start () {
		if (start_automatically) {
			StartTimer();
		}
	}

	void Update () {}

	void StartTimer ( float secs = -1) {
		if (secs < 0){
			secs = timerSecs;
		}
		Invoke ("Destruct", secs + Random.Range(-timerSecsVariance, timerSecsVariance));
	}

	void Destruct(){
		if (doRemoveEffects) {
			stTools.Remove(gameObject);
		}
		else {
			Destroy( gameObject);
		}
	}

	void AbortTimer (){
		CancelInvoke("Destruct");
	}
}
