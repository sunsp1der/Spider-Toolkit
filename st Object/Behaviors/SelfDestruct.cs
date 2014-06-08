using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Behaviors/Self Destruct")]


public class SelfDestruct : MonoBehaviour {
	// Remove object after given amount of time
	public float timerSecs = 3; // seconds before object self destructs
	public bool start_automatically = true; // start timer when object is created
											// otherwise call 'StartTimer' method
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
		Invoke ("Destruct", secs);
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
