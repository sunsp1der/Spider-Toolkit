using UnityEngine;
using System.Collections;

public class MouseClickSpawner : Spawner {

	[Tooltip("Keep spawning while key is down. Either way, Timing settings determine rate of fire")]
	public bool spawnWhilePressed = false;  
	
	bool readyToSpawn = true;

	void OnMouseDown(){
		if (readyToSpawn) {
			if (spawnWhilePressed) {
				StartSpawning();
				DelayNextSpawn();
			}
			else {
			 	CheckSpawn (false);
				DelayNextSpawn();
			}
		}
	}

	void OnMouseUp() {
		StopSpawning();

	}

	void OnMouseExit() {
		// stopspawning should work here but it gets called after click for no apparent reason

	}

	// don't spawn again til timing interval has passed
	void DelayNextSpawn() {
		// abort spawning until ReadySpawn is called
		readyToSpawn = false;
		// set timer to call ReadySpawn after timing interval
		Invoke ("ReadySpawn", timing.interval + Random.Range(-timing.intervalVariance, timing.intervalVariance));
	}
	
	// automatically called when spawn interval has passed. Cancels the effects of DelayNextSpawn
	void ReadySpawn() {
		readyToSpawn = true;
	}
}
