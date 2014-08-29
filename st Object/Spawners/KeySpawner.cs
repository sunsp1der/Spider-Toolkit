using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Spawners/Key Spawner")]

public class KeySpawner : Spawner {

	[Tooltip("See project settings/input")]
	public string inputName = "Fire1"; 
	[Tooltip("Keep spawning while key is down. Either way, Timing settings determine rate of fire")]
	public bool spawnWhilePressed = false;  

	bool readyToSpawn = true;

	void Update () {
		// spawn key pressed
		if (Input.GetButtonDown(inputName)) {

			if (spawnWhilePressed && readyToSpawn) {
				// start autospawning if we're doing key down autospawning
				StartSpawning();

				// start timer til next spawn 
				DelayNextSpawn();
			}
			else if (readyToSpawn) {
				// use checkspawn to respect the spawner Count settings
				CheckSpawn(false);

				// start timer til next spawn
				DelayNextSpawn();
			}
		}

		// spawn key released
		else if (spawnWhilePressed && Input.GetButtonUp(inputName)){
			// button released, so stop rapid firing
			StopSpawning();
		}
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