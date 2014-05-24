using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("st Object/Spawners/Auto Spawner")]


public class AutoSpawner : Spawner {
	// spawn objects ala Spawner component. 

	public bool autoStart = true; // Start spawning immediately. If false, call 'StartSpawning'
	
	void Start () {
		if (autoStart){ 
			StartSpawning(); // call this to start autospawning
		}
	}
}
