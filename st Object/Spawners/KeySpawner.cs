using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Spawners/Key Spawner")]

public class KeySpawner : Spawner {

	[Tooltip("See project settings/input")]
	public string inputName = "Fire1"; 
	[Tooltip("Keep spawning while key is down. Either way, Timing settings determine rate of fire")]
	public bool spawnWhilePressed = false;  

	bool readyToFire = true;

	void Update () {
		if (Input.GetButtonDown(inputName)) {
			if (spawnWhilePressed) {
				StartSpawning();
			}
			else if (readyToFire) {
				// use checkspawn to respect the spawner Count settings
				CheckSpawn(false);
				readyToFire = false;
				Invoke ("ReadyFire", timing.interval + Random.Range(-timing.intervalVariance, timing.intervalVariance));
			}
		}
		else if (spawnWhilePressed && Input.GetButtonUp(inputName)){
			StopSpawning();
		}
	}

	// used to conform to timing settings.
	void ReadyFire() {
		readyToFire = true;
	}
}

