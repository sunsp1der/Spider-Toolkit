using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Spawners/Key Spawner")]

public class KeySpawner : Spawner {

	public string inputName = "Fire1"; // see project settings/input
	public bool spawnWhilePressed = false; // keep spawning while key is down
	
	void Update () {
		if (Input.GetButtonDown(inputName)) {
			if (spawnWhilePressed) {
				StartSpawning();
			}
			else {
				Spawn ();
			}
		}
		else if (spawnWhilePressed && Input.GetButtonUp(inputName)){
			StopSpawning();
		}
	}
}

